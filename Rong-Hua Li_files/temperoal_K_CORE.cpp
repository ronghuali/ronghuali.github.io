
#include <sys/time.h>
#include <sys/types.h>
#include <bits/stdc++.h>

using namespace std;
const long TLE=100*1000;
const int max_num_of_node=1000;
typedef int32_t i32;

struct graph
{
    struct ds_t
    {
        i32 val;					//�洢degree-persistence
        vector<pair<i32, i32> > st;// first��MI�������˵㣬 second��Di��same degree
        ds_t() : val(), st() {}
    };
    // input parameters
    i32 theta, k, tau;
    // input data
    i32 V, E;
    struct node
    {
        i32 v;
        i32 t1_pos;//neighbor's id , and t1's pos in the ds[u].st; For optimization.
        i32 rev_u; // node u's position in neighbor[v]; For optimization.
        i32 t1,t2; // (t, t+theta )
        node() {}
        node(int a,int b,int c,int d)
        {
            v=a,rev_u=b,t1=c,t2=d;
        }
    };
    vector<vector<node>>neighbors;    // equal to  vector<node>neighbors[max_num_of_node];
//     vector<node>neighbors[max_num_of_node];    // equal to  vector<node>neighbors[max_num_of_node];

    // output data
    vector<ds_t> ds;        //�洢ÿ�����MI����
    vector<vector<i32> > ans;       //�洢��
    // helper data
    vector<bool> visited, is_selected, &in_comp;
    vector<i32> par;        //���鼯����
    // testing data
    timeval start_at, end_at;
    graph(istream& in, i32 theta, i32 k, i32 tau)
        : theta(theta),
          k(k),
          tau(tau),
          V(),
          E(),
          neighbors(),
          ans(),
          visited(),
          is_selected(),
          in_comp(visited),
          start_at(),
          end_at()
    {
        gettimeofday(&start_at, 0);
        assert(theta <= tau);
        in >> V >> E;

      //  neighbors.resize(V);
        for (i32 i = 0, last_t = 0; i < E; ++i)
        {
            i32 u, v, t;
            in >> t >> u >> v;
            assert(0 <= u && u < V);
            assert(0 <= v && v < V);
            assert(u != v);
            assert(last_t <= t);        // strict increasing
            vector <int>s;
            i32 bu = neighbors[u].size(), bv = neighbors[v].size();//��bv����u��v[]��λ��
            neighbors[u].emplace_back(v, bv,t, t + theta);/**/
            neighbors[v].emplace_back(u, bu, t, t + theta);/**/
            last_t = t;
        }
        gettimeofday(&end_at, 0);
        cout << "reading time: "
             << (end_at.tv_sec - start_at.tv_sec) * 1000 +
             (end_at.tv_usec - start_at.tv_usec) / 1000
             << "ms" << endl;
    }


    void deal_with_repeated_neighbor ( ) // NOTE:  �����ظ�����
    {
        vector<i32> last(V);//�洢u����һ���ھ���˭
        for (i32 u = 0; u < V; u++)
        {
            for (i32 i = 0; i < (i32)neighbors[u].size(); i++)
            {
                auto& n = neighbors[u][i];
                i32 j = last[n.v];//�洢u����һ���ھ�
                if (j < i && neighbors[u][j].v == n.v)//���������ͬ,����repeated�ھ�
                    if (neighbors[u][j].t2 > n.t1)//�����һ���ھӵ��Ҷ˵㳬���˵�ǰ�ھӵ���˵�,������ǰ��
                        neighbors[u][j].t2 = n.t1;//����j������
                last[n.v] = i;//������һ���ھ�
            }
        }
    }

    int Meta_Interval_Decomposition(i32 u)  //return num of MI
    {
        /*******merge sort **************/
        vector< pair<i32 ,i32 > > tmp;
        i32 neighbors_len=(i32)neighbors[u].size();
        for(i32 i=0;i<neighbors_len;i++)
                tmp.emplace_back(neighbors[u][i].t1,i),
                tmp.emplace_back(neighbors[u][i].t2,-1);
        sort(tmp.begin(),tmp.end());        //why not merge sort, because of 'deal_with_repeated_neighbor'
        /*******   ����MI �����Ż���Ԥ����,��¼t1_pos         **************/
        i32 ns=0,j=0,d=0,id;
        while(j<(i32)tmp.size()-1)
        {
            ds[u].st.emplace_back(tmp[j].first,0);
            id=tmp[j].second;
            d=(id!=-1?1:-1);
            if (id!=-1)
                neighbors[u][id] .t1_pos=ns;/* addtional optimization */
            while(j<(i32)tmp.size()-1 && tmp[j+1].first==tmp[j].first)
            {
                id=tmp[j+1].second;
                d=d+(id!=-1?1:-1),j++;
                if (id!=-1)
                    neighbors[u][id].t1_pos=ns; /* addtional optimization */
            }
            ds[u].st[ns].second=d,ns++;
            j++;
        }
        if (j==tmp.size()-1)        //deal with �߽����
        {
            ds[u].st.emplace_back(tmp[j].first,tmp[j].second);///*******************************
            ns++;
        }
        d=0;// Prefix sum
        for(i32 i=0; i<(i32)ds[u].st.size(); i++)
        {
            d=d+ds[u].st[i].second;
            ds[u].st[i].second=d;
        }
        ds[u].st.resize(ns);
        return ns;// num of MI
    }
    void compute_degree_persistence_f(i32 ns,i32 u)
    {
        for (i32 i = 0; i < ns - 1; i++)
        {
            int t1=ds[u].st[i].second;
            if (ds[u].st[i].second >= k)//ȡ maximal(theta,k)-persistent-degree interval
                ds[u].val += ds[u].st[i + 1].first - ds[u].st[i].first;//�ۼ�degree persistence-theta
        }
    }
    void enumerate_LPC()
    {
        vector<bool> visited(V);
        i32 cnt = 0, mx = 0;
        for (i32 u = 0; u < V; u++) //�ӵ�u����,BFS��չ�����ھ�
        {

            if (ds[u].val >= tau - theta && !visited[u]) //LPC�еĵ�,��δ������
            {
                vector<i32> comp;//�洢LPC
                i32 qh = 0;
                comp.emplace_back(u);
                visited[u] = true;
                while (qh < (i32)comp.size()) // search for LPC,
                {
                    i32 u = comp[qh];
                    qh++;
                    for (auto& n : neighbors[u])
                    {
                        i32 v = n.v;
                        if (!visited[v] && ds[v].val >= tau - theta)
                        {
                            comp.emplace_back(v);
                            visited[v] = true;
                        }
                    }
                }
                cnt++;
                mx = max((i32)comp.size(), mx);
            }
        }
        cout << "LPC 's num is "<< cnt << " and the maxsize is " << mx << endl;//output the num and maxsize of lpc
    }
    // NOTE: O(V+theta E)
    void cal_all_k_degree_nodes_with_stability_no_less_than_tau_per_theta()
    {
        gettimeofday(&start_at, 0);
        deal_with_repeated_neighbor(); // NOTE:  ���������ظ��ھ�
        queue<i32> q;       //���ڴ洢degree-persistence�ĵ�Ķ���,������չ,ɾ�����������ĵ�
        vector<bool> inq(V);    //��ǵ��Ƿ��ڶ�������
        for (i32 u = 0; u < V; u++)
        {
            if (neighbors[u].size() == 0)//���ھ���Ȼd-pΪ0
                ds[u].val = 0;
            else
            {
                /** Meta_Interval_Decomposition**/
                int ns=Meta_Interval_Decomposition(u);
                /** compute f(u,theta,k,G),that is degree-persistence *****/
                compute_degree_persistence_f(ns,u);
            }
            if (ds[u].val < tau - theta)  //�Ƚ�degree persistence��tau�Ĺ�ϵ,���С�����޳��õ�
            {
                inq[u] = true;          //��������
                q.emplace(u);           //���붪��������ɾ��
            }
        }
        /***************************** delete useless nodes               *************/
        while (!q.empty())//BFSɾ��dp������tau�ĵ�
        {
            i32 u = q.front();
            q.pop();
            for (auto& n : neighbors[u])
            {
                i32 v = n.v;
                i32 it=neighbors[v][n.rev_u].t1_pos;    // O��1��get the iterator
                //��u,v,t��
                // NOTE: O(theta)
                while (it<ds[v].st.size() && ds[v].st[it].first < n.t2)//while Tsj < Te
                {
                    ds[v].st[it].second--;// theta-p-d
                    if (ds[v].st[it].second + 1 >= k && ds[v].st[it].second < k) //�Ѿ���ȥ����û�ﵽ�ٽ�Ҫ��������Ҫ����
                    {
                        i32 delta = ds[v].st[it + 1].first - ds[v].st[it].first; //��delta����
                        ds[v].val -= delta;
                        if (ds[v].val + delta >= tau - theta && ds[v].val < tau - theta &&
                                !inq[v])  //����㲻�ڶ����У����ҷ���v ��dpС��tau,ɾ��
                        {
                            q.emplace(v);
                            inq[v] = true;
                        }
                    }
                    it++;
                }
            }
        }

        gettimeofday(&end_at, 0);
        cout << "preprocessing time: "
             << (end_at.tv_sec - start_at.tv_sec) * 1000 +
             (end_at.tv_usec - start_at.tv_usec) / 1000
             << "ms" << endl;

        /***************** enumerate   LPC  (just for show)   *****************/
        enumerate_LPC();      // just for Verification
    }

    i32 find(i32 x)  //���鼯��find
    {
        if (par[x] == x)   return x;
        return par[x]=find(par[x]);
    }

    pair<i32, i32> cal_stability_of_LPC(const vector<i32>& comp, i32 selected)
    {
        vector<pair<i32, i32>> st;
        st.emplace_back(make_pair(0, numeric_limits<i32>::max()));
        i32 len = numeric_limits<i32>::max(),
            selected_len = numeric_limits<i32>::max();
        /************************ �� Meta-Interval-Intersection   **********************/
        for (i32 i = 0; i < (i32)comp.size(); i++)  //����LPC�ĵ�
        {
            i32 u = comp[i];
            vector<pair<i32, i32>> nst; //��¼�õ��������st�ཻ��MI����
            i32 nlen = 0;
            auto it = st.begin();
            for (i32 j = 0; j < (i32)ds[u].st.size(); j++)  //����LPC�ĵ��MI����
            {
                if (ds[u].st[j].second >= k) // theta-p-d>=k
                {
                    // [ds[u].st[j].first, ds[u].st[j + 1].first);
                    while (it != st.end() && it->second <= ds[u].st[j].first)
                        it++;
                    while (it != st.end() && it->second <= ds[u].st[j + 1].first)
                    {
                        pair<i32, i32> range(max(it->first, ds[u].st[j].first), it->second);
                        if (range.first < range.second) //�����佻��
                        {
                            nst.emplace_back(range);
                            nlen += range.second - range.first;//�ۼ� meta-interval-intersection����
                        }
                        it++;
                    }
                    if (it != st.end() && it->first < ds[u].st[j + 1].first)    //����߽����
                    {
                        pair<i32, i32> range(max(it->first, ds[u].st[j].first),ds[u].st[j + 1].first);
                        if (range.first < range.second)
                        {
                            nst.emplace_back(range);
                            nlen += range.second - range.first;
                        }
                    }
                }
            }
            st.swap(nst);       //����  Meta-Interval-Intersection
            len = nlen;
            if (i < selected)       //���� F�Ͻ�,max cp ֵ
                selected_len = len;
            if (len < tau - theta)  //���core-persistence �Ѿ�������tau��
                break;

            if (i == selected - 1)  //��������selected���MI��,�жϸü����Ƿ�Ϸ�
            {
                // NOTE: important
                if (len >= tau - theta) //selected ���� GPC
                {
                    for (i32 i = 0; i < (i32)comp.size(); i++)  //�������鼯,��ʼ��
                    {
                        i32 u = comp[i];
                        par[u] = u;
                    }
                    for (i32 u : comp)
                    {
                        for (auto& n : neighbors[u])
                        {
                            i32 v = n.v; //u���ھӽڵ�v
                            if (in_comp[v])  //v��comp��
                            {
                                // NOTE: important
                                bool is_active = false;
                                i32 u_rev=n.rev_u;
                                i32 it=neighbors[u][neighbors[v][u_rev].rev_u].t1_pos;
                                i32 b = ds[u].st[it].first;
                                i32 e = n.t2;

                                while (ds[u].st[it].first < n.t2)
                                {
                                    if (ds[u].st[it].second >= k)
                                    {
                                        is_active = true;
                                        break;
                                    }
                                    it++;
                                }
                                if (is_active)
                                {
                                    i32 j =upper_bound(st.begin(), st.end(),make_pair(e, numeric_limits<i32>::max()))-st.begin() - 1; //��ʱ��st��¼�˴�0��selected -1�Ľڵ��Ԫ���佻�� ��j������st�����е�����ĵ�һ��Ԫ�ظպô���e���±�
                                    if (j!= -1 && b <= st[j].second)   //��b,e������ָ���Ǳߣ�u,v,t������Ӧ�����䣬���ڸ������а���һ��Ԫ���䣬�ڸ�Ԫ��������u�Ķȴ��ڵ���k
                                    {
                                        par[find(u)] = find(v); // ��U V�� st������ͨ��
                                    }
                                }
                            }
                        }
                    }
                    for (i32 i = 1; i < selected; i++)   //�����ѡ�Ľڵ㲻��ͬһ����ͨ��֧���棬��ֱ�ӷ��أ�-1��-1����ʾ����
                    {
                        i32 u = comp[i];
                        if (find(comp[0]) != find(u))
                            return make_pair(-1, -1);
                    }
                }
            }
        }
        return make_pair(len, selected_len);
    }
    bool remove_node(i32 u, queue<i32>& q)
    {
        bool removable = true;
        for (auto& n : neighbors[u])
        {
            i32 v = n.v;
            i32 it=neighbors[v][n.rev_u].t1_pos;
            while (ds[v].st[it].first < n.t2)
            {
                ds[v].st[it].second--;
                if (ds[v].st[it].second + 1 >= k && ds[v].st[it].second < k)
                {
                    i32 delta = ds[v].st[it + 1].first - ds[v].st[it].first;
                    ds[v].val -= delta;
                    if (ds[v].val + delta >= tau - theta && ds[v].val < tau - theta)
                    {
                        q.emplace(v);
                        if (is_selected[v])
                        {
                            removable = false;
                        }
                    }
                }
                it++;
            }
        }
        return removable;
    }
    void add_node(i32 u)
    {
        for (auto&n : neighbors[u])
        {
            i32 v = n.v;
            i32 it=neighbors[v][n.rev_u].t1_pos;
            while (ds[v].st[it].first < n.t2)
            {
                ds[v].st[it].second++;
                if (ds[v].st[it].second >= k && ds[v].st[it].second - 1 < k)
                {
                    i32 delta = ds[v].st[it + 1].first - ds[v].st[it].first;
                    ds[v].val += delta;
                }
                it++;
            }
        }
    }
    void branch_and_bound(const vector<i32>& comp, i32 selected)
    {
        static bool exit;
        gettimeofday(&end_at, 0);
        if (end_at.tv_sec - start_at.tv_sec > TLE)
            exit = true;
        if (exit)  return;
        // TODO break into connected components
        if (ans.back().size() >= comp.size()) return;  //��ǰsize�Ѿ�С����֪����,��֦
        // NOTE: important
        auto len = selected == 0 && comp.size() > 100 ? make_pair(-1, numeric_limits<i32>::max())
                   : cal_stability_of_LPC(comp, selected);

        if (len.first >= tau - theta)   // ���LPC�� core persistence ���ڵ���tau, �������һ��GPC
            ans.emplace_back(comp);
        else if (len.second >=  tau - theta && selected < (i32)comp.size())//���岻��GPC���Ӽ���cp����tau�����,�Ӽ��п�����GPC
        {
            // NOTE: important
            vector<bool> connected(comp.size(), true);
            if (selected)
            {
                for (i32 i = 0; i < (i32)comp.size(); i++)
                    connected[i] = find(comp[i]) == find(comp[0]); //��comp[i]��comp[0]��ͬһ����ͨ��֧�У���connected[i]Ϊtrue������Ϊfalse

            }
            vector<i32> compr, comps;
            i32 u = comp[selected];
            // remove u �����
            if (!exit)
            {
                // NOTE: important
                bool removable = true;
                vector<i32> removed;
                queue<i32> q;
                for (i32 i = 0; i < (i32)comp.size(); i++)
                {
                    i32 v = comp[i];
                    if (v == u || !connected[i])  //����ڵ�comp[i]��comp[0]����ͬһ��component���棬�����ɾ��
                        q.emplace(v);
                }
                while (!q.empty())      //delete U and corresponding nodes
                {
                    i32 v = q.front();
                    removed.emplace_back(v);
                    in_comp[v] = false;
                    if (!remove_node(v, q))
                    {
                        removable = false;      //failed to delete u
                        break;
                    }
                    q.pop();
                }
                if (removable)          // pick up the remaining nodes
                {
                    for (i32 v : comp)
                    {
                        if (in_comp[v])
                        {
                            compr.emplace_back(v);
                        }
                    }
                    branch_and_bound(compr, selected);      //sub
                }
                for (i32 v : removed)  //after ������including���������ɾ���ĵ�ӻ�ȥ
                {
                    in_comp[v] = true;
                    add_node(v);
                }
            }
            // select u �����
            if (!exit)
            {
                is_selected[u] = true;
                selected++;
                // NOTE: important
                bool selectable = true;
                vector<pair<i32, i32>> st;
                st.emplace_back(make_pair(0, numeric_limits<i32>::max()));
                i32 len = numeric_limits<i32>::max();
                for (i32 i = 0; i < selected; i++)  //��selected nodes ��   MI-intersection
                {
                    i32 v = comp[i];
                    vector<pair<i32, i32>> nst;
                    i32 nlen = 0;
                    auto it = st.begin();
                    for (i32 j = 0; j < (i32)ds[v].st.size(); j++)
                    {
                        if (ds[v].st[j].second >= k)
                        {
                            // [ds[v].st[j].first, ds[v].st[j + 1].first)
                            while (it != st.end() && it->second <= ds[v].st[j].first)
                            {
                                it++;
                            }
                            while (it != st.end() && it->second <= ds[v].st[j + 1].first)
                            {
                                pair<i32, i32> range(max(it->first, ds[v].st[j].first),
                                                     it->second);
                                if (range.first < range.second)
                                {
                                    nst.emplace_back(range);
                                    nlen += range.second - range.first;
                                }
                                it++;
                            }
                            if (it != st.end() && it->first < ds[v].st[j + 1].first)
                            {
                                pair<i32, i32> range(max(it->first, ds[v].st[j].first),
                                                     ds[v].st[j + 1].first);
                                if (range.first < range.second)
                                {
                                    nst.emplace_back(range);
                                    nlen += range.second - range.first;
                                }
                            }
                        }
                    }
                    st.swap(nst);       //���� MII
                    len = nlen;
                    if (len < tau - theta)  //���ֲ�����֮�󣬱�Ǹ��Ӽ�����ѡ
                    {
                        selectable = false;
                        break;
                    }
                }
                if (selectable)     //select���������������
                {
                    // NOTE: important
                    bool removable = true;
                    vector<i32> removed;
                    queue<i32> q;
                     for (i32 i = selected; i < (i32)comp.size(); i++)//try every node interested with set.st
                    {
                        i32 v = comp[i];
                        vector<pair<i32, i32>> nst;
                        i32 nlen = 0;
                        auto it = st.begin();
                        for (i32 j = 0; j < (i32)ds[v].st.size(); j++)
                        {
                            if (ds[v].st[j].second >= k)
                            {
                                // [ds[v].st[j].first, ds[v].st[j + 1].first)
                                while (it != st.end() && it->second <= ds[v].st[j].first)
                                    it++;
                                while (it != st.end() && it->second <= ds[v].st[j + 1].first)
                                {
                                    pair<i32, i32> range(max(it->first, ds[v].st[j].first),
                                                         it->second);
                                    if (range.first < range.second)
                                    {
                                        nst.emplace_back(range);
                                        nlen += range.second - range.first;
                                    }
                                    it++;
                                }
                                if (it != st.end() && it->first < ds[v].st[j + 1].first)
                                {
                                    pair<i32, i32> range(max(it->first, ds[v].st[j].first),
                                                         ds[v].st[j + 1].first);
                                    if (range.first < range.second)
                                    {
                                        nst.emplace_back(range);
                                        nlen += range.second - range.first;
                                    }
                                }
                            }
                        }
                        if (nlen < tau - theta || !connected[i])//�������S��v�ཻ������tau����������ͨ���֦
                            q.emplace(v);
                    }
                    while (!q.empty())
                    {
                        i32 v = q.front();
                        removed.emplace_back(v);
                        in_comp[v] = false;
                        if (!remove_node(v, q))
                        {
                            removable = false;
                            break;
                        }
                        q.pop();
                    }
                    if (removable)  //����ɾ�������������ռ�,�����ݹ�
                    {
                        for (i32 v : comp)
                        {
                            if (in_comp[v])
                            {
                                comps.emplace_back(v);
                            }
                        }
                        branch_and_bound(comps, selected);
                    }
                    // ��ԭ�ֳ�
                    for (i32 v : removed)
                    {
                        in_comp[v] = true;
                        add_node(v);
                    }
                }
                selected--;
                is_selected[u] = false;
            }
        }
    }
    void solve()
    {
        ds.resize(V);
        cal_all_k_degree_nodes_with_stability_no_less_than_tau_per_theta();
        visited.resize(V);
        is_selected.resize(V);
        par.resize(V);
        ans.emplace_back();
        gettimeofday(&start_at, 0);
        for (i32 u = 0; u < V; u++) //enumerate all the LPC
        {
            if (ds[u].val >= tau - theta && !visited[u])//���u��dp>=tau��δ�����ʹ�,���Դ˳�������LPC
            {
                vector<i32> comp;
                i32 qh = 0;
                comp.emplace_back(u);
                visited[u] = true;
                while (qh < (i32)comp.size())//��ÿһ�������,BFS��չ�����ھ�
                {
                    i32 u = comp[qh];
                    qh++;
                    for (auto& n : neighbors[u])//��������u��Ϸ�����ھ�
                    {
                        i32 v = n.v;//�ھ�
                        if (!visited[v] && ds[v].val >= tau - theta)//����ھ�Ҳ��һ��dp>Tau�ĵ�,�����LPC
                        {
                            comp.emplace_back(v);
                            visited[v] = true;
                        }
                    }
                }
                random_shuffle(comp.begin(), comp.end());//���������
                branch_and_bound(comp, 0);              //��LPC������GPC
            }
        }
        gettimeofday(&end_at, 0);
        cout << "searching time: "
             << (end_at.tv_sec - start_at.tv_sec) * 1000 +
             (end_at.tv_usec - start_at.tv_usec) / 1000
             << "ms" << endl;

        cout << "the largest GPC size is : "<<ans.back().size() << endl
        <<"which consist of the following nodes:"<<endl;
        for (i32 u : ans.back())
            cout << u << endl;
    }
};

int main(int argc, char* argv[])
{
    freopen("res_[test].txt","w",stdout);
    argv[1]="2.txt";        //data_path
    argv[2]="103";              //theta
    argv[3]="2";                //k
    argv[4]="107";                //tau
    ifstream in(argv[1]);
    i32 theta = atoi(argv[2]);
    i32 k = atoi(argv[3]);
    i32 tau = atoi(argv[4]);
    graph G(in, theta, k, tau);
    G.solve();
    return 0;
}

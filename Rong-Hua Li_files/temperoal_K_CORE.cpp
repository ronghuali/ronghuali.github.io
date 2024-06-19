
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
        i32 val;					//存储degree-persistence
        vector<pair<i32, i32> > st;// first是MI区间的左端点， second是Di：same degree
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
    vector<ds_t> ds;        //存储每个点的MI区间
    vector<vector<i32> > ans;       //存储答案
    // helper data
    vector<bool> visited, is_selected, &in_comp;
    vector<i32> par;        //并查集数组
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
            i32 bu = neighbors[u].size(), bv = neighbors[v].size();//在bv表在u中v[]的位置
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


    void deal_with_repeated_neighbor ( ) // NOTE:  处理重复区间
    {
        vector<i32> last(V);//存储u的上一个邻居是谁
        for (i32 u = 0; u < V; u++)
        {
            for (i32 i = 0; i < (i32)neighbors[u].size(); i++)
            {
                auto& n = neighbors[u][i];
                i32 j = last[n.v];//存储u的上一个邻居
                if (j < i && neighbors[u][j].v == n.v)//如果两个相同,则处理repeated邻居
                    if (neighbors[u][j].t2 > n.t1)//如果上一个邻居的右端点超过了当前邻居的左端点,则修正前者
                        neighbors[u][j].t2 = n.t1;//修正j的区间
                last[n.v] = i;//记下上一个邻居
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
        /*******   计算MI 并作优化的预处理,记录t1_pos         **************/
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
        if (j==tmp.size()-1)        //deal with 边界情况
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
            if (ds[u].st[i].second >= k)//取 maximal(theta,k)-persistent-degree interval
                ds[u].val += ds[u].st[i + 1].first - ds[u].st[i].first;//累计degree persistence-theta
        }
    }
    void enumerate_LPC()
    {
        vector<bool> visited(V);
        i32 cnt = 0, mx = 0;
        for (i32 u = 0; u < V; u++) //从点u出发,BFS扩展所有邻居
        {

            if (ds[u].val >= tau - theta && !visited[u]) //LPC中的点,且未被访问
            {
                vector<i32> comp;//存储LPC
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
        deal_with_repeated_neighbor(); // NOTE:  处理相邻重复邻居
        queue<i32> q;       //用于存储degree-persistence的点的队列,不断扩展,删除所有这样的点
        vector<bool> inq(V);    //标记点是否在队列里面
        for (i32 u = 0; u < V; u++)
        {
            if (neighbors[u].size() == 0)//无邻居显然d-p为0
                ds[u].val = 0;
            else
            {
                /** Meta_Interval_Decomposition**/
                int ns=Meta_Interval_Decomposition(u);
                /** compute f(u,theta,k,G),that is degree-persistence *****/
                compute_degree_persistence_f(ns,u);
            }
            if (ds[u].val < tau - theta)  //比较degree persistence和tau的关系,如果小于则剔除该点
            {
                inq[u] = true;          //标记已入队
                q.emplace(u);           //丢入丢掉继续被删除
            }
        }
        /***************************** delete useless nodes               *************/
        while (!q.empty())//BFS删掉dp不超过tau的点
        {
            i32 u = q.front();
            q.pop();
            for (auto& n : neighbors[u])
            {
                i32 v = n.v;
                i32 it=neighbors[v][n.rev_u].t1_pos;    // O（1）get the iterator
                //（u,v,t）
                // NOTE: O(theta)
                while (it<ds[v].st.size() && ds[v].st[it].first < n.t2)//while Tsj < Te
                {
                    ds[v].st[it].second--;// theta-p-d
                    if (ds[v].st[it].second + 1 >= k && ds[v].st[it].second < k) //已经减去过或还没达到临界要减都不需要进入
                    {
                        i32 delta = ds[v].st[it + 1].first - ds[v].st[it].first; //算delta区间
                        ds[v].val -= delta;
                        if (ds[v].val + delta >= tau - theta && ds[v].val < tau - theta &&
                                !inq[v])  //如果点不在队列中，并且发现v 的dp小于tau,删除
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

    i32 find(i32 x)  //并查集的find
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
        /************************ 求 Meta-Interval-Intersection   **********************/
        for (i32 i = 0; i < (i32)comp.size(); i++)  //遍历LPC的点
        {
            i32 u = comp[i];
            vector<pair<i32, i32>> nst; //记录该点的所有与st相交的MI区间
            i32 nlen = 0;
            auto it = st.begin();
            for (i32 j = 0; j < (i32)ds[u].st.size(); j++)  //遍历LPC的点的MI区间
            {
                if (ds[u].st[j].second >= k) // theta-p-d>=k
                {
                    // [ds[u].st[j].first, ds[u].st[j + 1].first);
                    while (it != st.end() && it->second <= ds[u].st[j].first)
                        it++;
                    while (it != st.end() && it->second <= ds[u].st[j + 1].first)
                    {
                        pair<i32, i32> range(max(it->first, ds[u].st[j].first), it->second);
                        if (range.first < range.second) //求区间交集
                        {
                            nst.emplace_back(range);
                            nlen += range.second - range.first;//累计 meta-interval-intersection长度
                        }
                        it++;
                    }
                    if (it != st.end() && it->first < ds[u].st[j + 1].first)    //处理边界情况
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
            st.swap(nst);       //更新  Meta-Interval-Intersection
            len = nlen;
            if (i < selected)       //更新 F上界,max cp 值
                selected_len = len;
            if (len < tau - theta)  //如果core-persistence 已经不超过tau了
                break;

            if (i == selected - 1)  //求完所有selected点的MI后,判断该集合是否合法
            {
                // NOTE: important
                if (len >= tau - theta) //selected 构成 GPC
                {
                    for (i32 i = 0; i < (i32)comp.size(); i++)  //建立并查集,初始化
                    {
                        i32 u = comp[i];
                        par[u] = u;
                    }
                    for (i32 u : comp)
                    {
                        for (auto& n : neighbors[u])
                        {
                            i32 v = n.v; //u的邻居节点v
                            if (in_comp[v])  //v在comp中
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
                                    i32 j =upper_bound(st.begin(), st.end(),make_pair(e, numeric_limits<i32>::max()))-st.begin() - 1; //此时的st记录了从0到selected -1的节点的元区间交集 ；j代表了st数组中的区间的第一个元素刚好大于e的下标
                                    if (j!= -1 && b <= st[j].second)   //（b,e）区间指的是边（u,v,t）所对应的区间，且在该区间中包含一个元区间，在该元区间中有u的度大于等于k
                                    {
                                        par[find(u)] = find(v); // 则U V在 st中是联通的
                                    }
                                }
                            }
                        }
                    }
                    for (i32 i = 1; i < selected; i++)   //如果必选的节点不在同一个连通分支里面，则直接返回（-1，-1）表示剪掉
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
        if (ans.back().size() >= comp.size()) return;  //当前size已经小于已知最大答案,剪枝
        // NOTE: important
        auto len = selected == 0 && comp.size() > 100 ? make_pair(-1, numeric_limits<i32>::max())
                   : cal_stability_of_LPC(comp, selected);

        if (len.first >= tau - theta)   // 如果LPC的 core persistence 大于等于tau, 则这就是一个GPC
            ans.emplace_back(comp);
        else if (len.second >=  tau - theta && selected < (i32)comp.size())//整体不是GPC·子集的cp大于tau的情况,子集有可能是GPC
        {
            // NOTE: important
            vector<bool> connected(comp.size(), true);
            if (selected)
            {
                for (i32 i = 0; i < (i32)comp.size(); i++)
                    connected[i] = find(comp[i]) == find(comp[0]); //若comp[i]与comp[0]在同一个连通分支中，则connected[i]为true，否则为false

            }
            vector<i32> compr, comps;
            i32 u = comp[selected];
            // remove u 的情况
            if (!exit)
            {
                // NOTE: important
                bool removable = true;
                vector<i32> removed;
                queue<i32> q;
                for (i32 i = 0; i < (i32)comp.size(); i++)
                {
                    i32 v = comp[i];
                    if (v == u || !connected[i])  //如果节点comp[i]与comp[0]不在同一个component里面，则可以删除
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
                for (i32 v : removed)  //after 讨论了including的情况，把删过的点加回去
                {
                    in_comp[v] = true;
                    add_node(v);
                }
            }
            // select u 的情况
            if (!exit)
            {
                is_selected[u] = true;
                selected++;
                // NOTE: important
                bool selectable = true;
                vector<pair<i32, i32>> st;
                st.emplace_back(make_pair(0, numeric_limits<i32>::max()));
                i32 len = numeric_limits<i32>::max();
                for (i32 i = 0; i < selected; i++)  //求selected nodes 的   MI-intersection
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
                    st.swap(nst);       //更新 MII
                    len = nlen;
                    if (len < tau - theta)  //发现不满足之后，标记该子集不可选
                    {
                        selectable = false;
                        break;
                    }
                }
                if (selectable)     //select满足条件的情况下
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
                        if (nlen < tau - theta || !connected[i])//如果集合S和v相交后不满足tau条件或者连通则剪枝
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
                    if (removable)  //可以删除，缩减搜索空间,继续递归
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
                    // 还原现场
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
            if (ds[u].val >= tau - theta && !visited[u])//如果u的dp>=tau且未被访问过,则以此出发搜索LPC
            {
                vector<i32> comp;
                i32 qh = 0;
                comp.emplace_back(u);
                visited[u] = true;
                while (qh < (i32)comp.size())//从每一个点出发,BFS扩展所有邻居
                {
                    i32 u = comp[qh];
                    qh++;
                    for (auto& n : neighbors[u])//遍历所有u点合法点的邻居
                    {
                        i32 v = n.v;//邻居
                        if (!visited[v] && ds[v].val >= tau - theta)//如果邻居也是一个dp>Tau的点,则加入LPC
                        {
                            comp.emplace_back(v);
                            visited[v] = true;
                        }
                    }
                }
                random_shuffle(comp.begin(), comp.end());//随机化序列
                branch_and_bound(comp, 0);              //在LPC中搜搜GPC
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

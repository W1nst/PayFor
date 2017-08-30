export default {
    install(Vue, options) {
      Vue.prototype.$auth = new Vue({
        data: function() {
          return {
            user: undefined,
            token: undefined,
            isAuthenticated: false
          }
        },
        created: function() {
            var vm = this;
            vm.setAuth();

            vm.$http.interceptors.response.use(function (response) {
                // Do something with response data
                return response;
            }, function (error) {
                //console.log (error);
                // Do something with response error
                return Promise.reject(error);
            });
            // vm.$http.interceptors.response.use(null,function(error) {
            //     console.log (error);
            //     if (error.status === 401) {
            //         console.log ('WAS');
            //         options.router.push({ name: 'login'});
            //         return defaultResponse;
            //     }
            //     return Promise.reject(error);
            // });
            ApplyRouteGuard.call(this, options.router);
        },
        methods: {
            login(loginModel) {
                var vm = this;
                return new Promise(function(resolve, reject) {
                    vm.$http.post(vm.$apiHelper.generateTokenUrl(), loginModel)
                    .then(function(response){
                        vm.storeAuthData(response.data);
                        vm.setAuth();    
                        resolve(true);
                    }).catch(function(ex){
                        vm.logout();
                        resolve(false);
                    });
                });
            },
            storeAuthData(data){
                sessionStorage.setItem('token', data.token);
                sessionStorage.setItem('user', data.name);
            },
            setAuth(){
                var vm = this;
                var token = sessionStorage.getItem('token');
                var name = sessionStorage.getItem('user');
                if(token && name) {
                    vm.isAuthenticated = true;        
                    vm.token = token;
                    vm.user = name;
                    //vm.$http.defaults.headers.common['Authorization'] = 'Bearer ' + token;
                }
                else {
                    vm.logout();   
                }
            },
            logout() {
                var vm = this;
                sessionStorage.removeItem('token')
                sessionStorage.removeItem('user')
                vm.isAuthenticated = false;        
                vm.token = undefined;
                vm.user = undefined;
                vm.$http.defaults.headers.common['Authorization'] = undefined;
                options.router.push({ name: 'login'});
            }
        }
      });
    }
  };
  
function ApplyRouteGuard(router) {
    router.beforeEach((to, from, next) => {
        if (to.matched.some(record => record.meta.auth == true)) {
            if (!this.isAuthenticated) {
                next({name: 'login'})
            } else {
                next()
            }
        } else if (to.matched.some(record => record.meta.auth == false)){
            if (this.isAuthenticated) {
                next({name: 'groups'})
            } else {
                next();
            }
        } else {
            next();
        }
    });
}
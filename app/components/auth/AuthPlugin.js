export default {
    install(Vue, options) {
      Vue.prototype.$auth = new Vue({
        data: function() {
          return {
            token: undefined,
            isAuthenticated: false
          }
        },
        created: function() {
            var vm = this;
            vm.setAuth();
            vm.$http.interceptors.response.use(null,function(error) {
                if (error.response.status === 401) {
                    vm.logout();
                    return defaultResponse;
                }
                return Promise.reject(error);
            });
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
                    }).catch(function(error){
                        reject(error);
                    });
                });
            },
            signUp(signUpModel){
                var vm = this;
                return new Promise(function(resolve, reject) {
                    vm.$http.post(vm.$apiHelper.signUpUrl(), signUpModel)
                    .then(function(response){
                        vm.storeAuthData(response.data);
                        vm.setAuth();    
                        resolve(true);
                    }).catch(function(error){
                        reject(error);
                    });
                });
            },
            storeAuthData(data){
                sessionStorage.setItem('token', data.token);
            },
            setAuth(){
                var vm = this;
                var token = sessionStorage.getItem('token');
                if(token) {
                    vm.isAuthenticated = true;        
                    vm.token = token;
                    vm.$http.defaults.headers.common['Authorization'] = 'Bearer ' + token;
                }
                else {
                    vm.logout();   
                }
            },
            logout() {
                var vm = this;
                sessionStorage.removeItem('token');
                vm.isAuthenticated = false;        
                vm.token = undefined;
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
                next();
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
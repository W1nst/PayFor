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
            this.setAuth();
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
                localStorage.setItem('token', data.token);
                localStorage.setItem('user', data.name);
            },
            setAuth(){
                var vm = this;
                var token = localStorage.getItem('token');
                var name = localStorage.getItem('user');
                if(token && name) {
                    vm.isAuthenticated = true;        
                    vm.token = token;
                    vm.user = name;
                    vm.$http.defaults.headers.common['Authorization'] = 'Bearer ' + token;
                }
                else {
                    vm.logout();   
                }
            },
            logout() {
                var vm = this;
                localStorage.removeItem('token')
                localStorage.removeItem('user')
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
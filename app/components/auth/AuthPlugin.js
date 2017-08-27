export default {
    install(Vue, options) {
      Vue.prototype.$auth = new Vue({
        data: function() {
          return {
            user: undefined,
            token: undefined,
          }
        },
        created: function() {
          ApplyRouteGuard.call(this, options.router);
        },
        methods: {
            login(loginModel) {
                var vm = this;
                return new Promise(function(resolve, reject) {
                    console.log(loginModel);
                    vm.$http.post(vm.$apiHelper.generateTokenUrl(), loginModel)
                    .then(function(response){
                        vm.token = response.data.token;
                        vm.user = response.data.name;
                        vm.$http.defaults.headers.common['Authorization'] = 'Bearer '+response.data.token;
                        resolve(true);
                    }).catch(function(ex){
                        vm.token = undefined;
                        vm.user = undefined;
                        vm.$http.defaults.headers.common['Authorization'] = undefined;
                        resolve(false);
                    });
                });
            },
            logout() {
                vm.token = undefined;
                vm.user = undefined;
            }
        }
      });
    }
  };
  
function ApplyRouteGuard(router) {
    router.beforeEach((to, from, next) => {
        let route = to.matched.find(e => e.meta.auth != null);
        console.log(route);
        if (route) {
            let auth = route.meta.auth;
            if (auth && !this.user) {
                console.log('Access denied - only for authenticated users:', route.path);
                //TODO: redirect to the login page
                router.push({ name: 'login'});
            } else if (!auth && this.user) {
                console.log('Hide from authenticated users - only for guests:', route.path);
                //TODO: disable, hide somehow
            } else {
                console.log('Appropriate route, user/guset has rights to visit:', route.path);
            }
        }
        next();
    });
}
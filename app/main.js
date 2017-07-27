import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

import isBusy from './components/shared/isBusy.vue';
import mainwrapper from './components/mainwrapper.vue';
import groups from './components/group/groups.vue';
import groupRow from './components/group/groupRow.vue';
import createGroup from './components/group/createGroup.vue';
import group from './components/group/group.vue';
import sidebar from './components/sidebar.vue';

Vue.component('sidebar', sidebar);
Vue.component('mainwrapper', mainwrapper);
Vue.component('isBusy', isBusy);
Vue.component('groups', groups);
Vue.component('grouprow', groupRow);
Vue.component('creategroup', createGroup);
Vue.component('group', group);
Vue.filter('moment', require('./filters/moment'));


const routes = new VueRouter({
    routes:[
        {path:'/groups', component:groups},
        {path:'/creategroup', component:createGroup}
    ]
});

const app = new Vue({
    el:'#app',
    routes:routes
})

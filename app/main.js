import Vue from 'vue';

import isBusy from './components/shared/isBusy.vue';
import mainwrapper from './components/mainwrapper.vue';
import groups from './components/group/groups.vue';
import groupRow from './components/group/groupRow.vue';
import createGroup from './components/group/createGroup.vue';
import group from './components/group/group.vue';

Vue.component('isBusy', isBusy);
Vue.component('groups', groups);
Vue.component('grouprow', groupRow);
Vue.component('creategroup', createGroup);
Vue.component('group', group);
Vue.filter('moment', require('./filters/moment'));

const app = new Vue({
    el:'#app',
    components:{
        mainwrapper: mainwrapper
    }
})

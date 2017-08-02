import Vue from 'vue'
import VueRouter from 'vue-router'
import VeeValidate from 'vee-validate'
import axios from 'axios'
import bootstrapdatepicker from 'bootstrap-datepicker'
import moment from 'moment'

window.moment = moment;

Vue.use(VueRouter)
Vue.use(VeeValidate)
Vue.prototype.$http = axios
Vue.prototype.$moment = moment

import isBusy from './components/shared/isBusy.vue'
import mainwrapper from './components/mainwrapper.vue'
import groups from './components/group/groups.vue'
import groupRow from './components/group/groupRow.vue'
import createGroup from './components/group/createGroup.vue'
import group from './components/group/group.vue'
import sidebar from './components/sidebar.vue'
import paymentrow from './components/payments/paymentRow.vue'
import createpayment from './components/payments/createPayment.vue'


Vue.component('sidebar', sidebar);
Vue.component('mainwrapper', mainwrapper);
Vue.component('isBusy', isBusy);
Vue.component('groups', groups);
Vue.component('grouprow', groupRow);
Vue.component('creategroup', createGroup);
Vue.component('group', group);
Vue.component('paymentrow',paymentrow);
Vue.component('createpayment',createpayment);



Vue.filter('moment', require('./filters/moment'));


const router = new VueRouter({
    routes:[
        {   
            name: 'groups',
            path: '/groups',          
            component: groups
        },{   
            name: 'creategroup',
            path: '/creategroup',     
            component: createGroup 
        },{   
            name: 'group',
            path: '/group/:groupId',       
            component: group,
            props:true,
            children:[{
                name:'createpayment',
                path:'createpayment',
                component: createpayment
            }]
        }
    ]
});

const app = new Vue({
    router
}).$mount('#app');

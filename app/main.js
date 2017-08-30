import Vue from 'vue'
import VueRouter from 'vue-router'
import VeeValidate from 'vee-validate'
import axios from 'axios'
import bootstrapdatepicker from 'bootstrap-datepicker'
import moment from 'moment'
import config from './config'
import apiHelper from './helpers/apihelper'
import authPlugin from './components/auth/AuthPlugin'

window.moment = moment;

Vue.use(VueRouter)
Vue.use(VeeValidate)

Vue.prototype.$http = axios
Vue.prototype.$moment = moment
Vue.prototype.$appConfig = config
Vue.prototype.$apiHelper = apiHelper

import isBusy from './components/shared/isBusy.vue'
import mainwrapper from './components/mainwrapper.vue'
import groups from './components/group/groups.vue'
import groupRow from './components/group/groupRow.vue'
import createGroup from './components/group/createGroup.vue'
import group from './components/group/group.vue'
import sidebar from './components/sidebar.vue'
import paymentrow from './components/payments/paymentRow.vue'
import createpayment from './components/payments/createPayment.vue'
import login from './components/auth/login.vue'
import uheader from './components/header.vue'

Vue.component('sidebar', sidebar);
Vue.component('uheader', uheader);
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
    //mode: 'history',
    routes:[
        {
            name:'index',
            path:'/',
            meta: { auth: true  },
            component: groups
        },{   
            name: 'login',
            path: '/login',    
            meta: { auth: false  },      
            component: login
        },{   
            name: 'groups',
            path: '/groups',    
            meta: { auth: true  },      
            component: groups
        },{   
            name: 'creategroup',
            path: '/creategroup',   
            meta: { auth: true  },  
            component: createGroup 
        },{   
            name: 'group',
            path: '/group/:groupId',       
            component: group,
            meta: { auth: true  },
            props:true,
            children:[{
                    name:'addpayment',
                    path:'addpayment',
                    component: createpayment
                },{
                    name:'editpayment',
                    path:'editpayment/:paymentId',
                    props: true,
                    component: createpayment
                }

            ]
        }
    ]
});

Vue.use(authPlugin,{router: router});

const app = new Vue({
    router
}).$mount('#app');

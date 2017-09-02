<template>
    <article class="dashboard-page">
        <div class="title-block">
            <h1 class="title"> {{group.name}} </h1>
            <p class="title-description" v-show="authorName"> Author: {{authorName}} </p>
        </div>
        <section class="section">
            <div class="row">
                <div class="col-xl-9">
                    <div class="card items" data-exclude="xs,sm,lg">
                        <div class="card-header bordered">
                            <div class="header-block">
                                <h3 class="title">Payments</h3> <router-link :to="{ name: 'addpayment'}" class="btn btn-primary btn-sm rounded">Add new</router-link> 
                            </div>
                        </div>
                        <transition name="slide-fade" mode="out-in">
                            <router-view :key="$route.fullPath"></router-view>
                        </transition>
                        <ul class="item-list striped">
                            <li class="item item-list-header hidden-sm-down">
                                <div class="item-row">
                                    <div class="item-col item-col-header item-col-title">
                                        <div> <span>Note</span> </div>
                                    </div>
                                    <div class="item-col item-col-header item-col-sales">
                                        <div> <span>Amount</span> </div>
                                    </div>
                                    <div class="item-col item-col-header item-col-category">
                                        <div class="no-overflow"> <span>Category</span> </div>
                                    </div>
                                    <div class="item-col item-col-header item-col-author">
                                        <div class="no-overflow"> <span>Author</span> </div>
                                    </div>
                                    <div class="item-col item-col-header item-col-date">
                                        <div class="no-overflow"> <span>Payment date</span> </div>
                                    </div>
                                    <div class="item-col item-col-header fixed item-col-actions-dropdown"> </div>
                                </div>
                            </li>
                            <li class="item"
                                is="paymentrow" 
                                v-for="(payment,index) in group.payments" 
                                v-bind:payment="payment" 
                                v-bind:key="index" 
                                v-on:remove="deletePayment(index)">
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="col col-xs-12 col-sm-12 col-md-12 col-xl-3 stats-col">
                    <div class="card stats" data-exclude="xs">
                        <div class="card-block">
                            <div class="title-block">
                                <h4 class="title"> Stats </h4>
                                <p class="title-description"> Group metrics </p>
                            </div>
                            <div class="row row-sm stats-container">
                                <div class="col-xs-12 stat-col">
                                    <div class="stat-icon"> <i class="fa fa-shopping-cart"></i> </div>
                                    <div class="stat">
                                        <div class="value"> {{paymentscount}} </div>
                                        <div class="name">Payments</div>
                                    </div> 
                                    <progress class="progress stat-progress" value="25" max="100">
                                        <div class="progress">
                                            <span class="progress-bar" style="width: 25%;"></span>
                                        </div>
                                    </progress> 
                                </div>
                                <div class="col-xs-12 col-sm-12  stat-col">
                                    <div class="stat-icon"> <i class="fa fa-line-chart"></i> </div>
                                    <div class="stat">
                                        <div class="value"> {{monthlyexpenses}} </div>
                                        <div class="name">Monthly expenses</div>
                                    </div> 
                                    <progress class="progress stat-progress" value="60" max="100">
                                        <div class="progress">
                                            <span class="progress-bar" style="width: 60%;"></span>
                                        </div>
                                    </progress>
                                </div>
                                <div class="col-xs-12 col-sm-12  stat-col">
                                    <div class="stat-icon"> <i class="fa fa-users"></i> </div>
                                    <div class="stat">
                                        <div class="value"> {{userscount}} </div>
                                        <div class="name"> Total users </div>
                                    </div> 
                                    <progress class="progress stat-progress" value="34" max="100">
                                        <div class="progress">
                                            <span class="progress-bar" style="width: 34%;"></span>
                                        </div>
                                    </progress> 
                                </div>                                
                                <div class="col-xs-12 col-sm-12 stat-col">
                                    <div class="stat-icon"> <i class="fa fa-dollar"></i> </div>
                                    <div class="stat">
                                        <div class="value"> {{total}} </div>
                                        <div class="name"> Total </div>
                                    </div> 
                                    <progress class="progress stat-progress" value="15" max="100">
                                        <div class="progress">
                                            <span class="progress-bar" style="width: 15%;"></span>
                                        </div>
                                    </progress> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                
            </div>
        </section>        
    </article>
</template>

<script>
    //var moment = require('moment');
    export default {
        props : ['groupId'],
        data () {
            return {
                group:{},
                loaded:false,
                errorMessage:''
            }
        },
        computed: {
            total: function(){
                if (this.loaded){
                    return this.group.payments.reduce(function(prev, payment){
                        return prev + payment.amount; 
                    },0);
                }else{
                    return 0;
                }
            },
            monthlyexpenses: function(){
                var vm = this;
                if (vm.loaded){
                    return vm.group.payments.
                        filter(function(payment){
                            var paymentdate = vm.$moment(payment.date);
                            var currentdate = vm.$moment();
                            return paymentdate.month() == currentdate.month() 
                                   && paymentdate.year() == currentdate.year()})
                        .reduce(function(prev, payment){
                            return prev + payment.amount;
                        },0);
                } else{
                    return 0;
                }
            },
            paymentscount: function(){
                if (this.loaded){
                    return this.group.payments.length;
                } else {
                    return 0;
                }
            },
            userscount: function(){
                if (this.loaded){
                    return this.group.users.length;
                } else {
                    return 0;
                } 
            },
            authorName: function(){
                if (this.loaded && this.group.authorUser){
                    return this.group.authorUser.firstName + ' ' +  this.group.authorUser.lastName;
                } else {
                    return '';
                } 
            }
        },
        methods: {
            fetchGroup: function(){
                var vm = this;
                vm.$http.get(vm.$apiHelper.getGroupUrl(vm.groupId))
                    .then(function (response) {
                        vm.group = response.data;
                        vm.loaded = true;
                    })
                    .catch(function (error) {
                        console.log(error.response.data.message); 
                        vm.$router.push('/groups');   
                    });
            },
            deletePayment:function(index){
                var vm = this;
                vm.$swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this payment: " + vm.group.payments[index].note + "!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false,
                        showLoaderOnConfirm: true,
                    },
                    function(){
                        vm.errorMessage = "";
                        vm.$http.post(vm.$apiHelper.deletePaymentUrl(vm.group.payments[index].id))
                        .then(function(response){
                            vm.group.payments.splice(index, 1);
                            swal("Deleted!", "Your group has been deleted.", "success");
                        }).catch(function(error){
                            console.log(error.response.data.message);
                            vm.errorMessage =  error.response.data.message;
                            swal("Error", vm.errorMessage, "error");
                        }); 
                });
            },
        },
        created:function(){
            this.fetchGroup();
        },
        watch:{
            '$route':'fetchGroup'
        }  
    }
</script>
<style>

.slide-fade-leave-active, .slide-fade-enter-active {
    transition: 0.3s ease;
}
.slide-fade-enter, .slide-fade-leave-to{
    max-height: 0px;
    overflow: hidden;
}
.slide-fade-enter-to, .slide-fade-leave{
    max-height: 1500px;
    overflow: hidden;
}
</style>
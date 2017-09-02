<template>
    <div>
        <div class="row">
            <div class="col-xl-4">
                <div class="card" data-exclude="xs,sm,lg">
                    <div class="card-header">
                        <div class="header-block">
                            <h3 class="title"> User information </h3>
                        </div>
                    </div>
                    <div class="card-block">
                        <form role="form">
                            <fieldset class="form-group"> 
                                <label class="control-label" for="firstname">First Name</label>
                                <p class="form-control-static" id="firstname">{{user.firstName}}</p>
                            </fieldset>
                            <fieldset class="form-group"> 
                                <label class="control-label" for="lastname">Last Name</label>
                                <p class="form-control-static" id="lastname">{{user.lastName}}</p>
                            </fieldset>
                            <fieldset class="form-group"> 
                                <label class="control-label" for="email">Email</label>
                                <p class="form-control-static" id="email">{{user.email}}</p>
                            </fieldset>
                        </form>
                    </div>
                </div>
            </div>
            <div class="col-xl-8">
                <div class="card" data-exclude="xs,sm,lg">
                    <div class="card-header">
                        <div class="header-block">
                            <h3 class="title"> User groups </h3>
                        </div>
                    </div>
                    <div class="card-block">
                        <grouplist v-if="loaded" v-bind:usergroups="user.groups" v-bind:editable="false"></grouplist>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xl-12">
                <div class="card" data-exclude="xs,sm,lg">
                    <div class="card-header">
                        <div class="header-block">
                            <h3 class="title"> User payments </h3>
                        </div>
                    </div>
                    <div class="card-block" v-if="loaded">
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
                                v-for="(payment,index) in user.payments" 
                                v-bind:payment="payment" 
                                v-bind:key="index">
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    data:function(){
        return {
            user:{
                firstName:'',
                lastName:'',
                email:'',
                groups:[],
                payments:[]
            },
            loaded:false
        }
    },
    methods:{
        fetchData: function(){
            var vm = this;
            vm.$http.get(vm.$apiHelper.userBaseUrl)
                .then(function (response) {
                    vm.user = response.data;
                    vm.loaded = true;
                })
                .catch(function (error) {
                    console.log(error.response.data.message); 
                    vm.$router.push('/groups');   
                });
        }
    },
    created(){
        this.fetchData();
    }
}
</script>

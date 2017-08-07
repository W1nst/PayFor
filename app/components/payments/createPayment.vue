<template>
    <div class="card-header bordered" id="pEditor">
        <div class="col-md-12">
            <div class="card card-block">
                <div class="title-block">
                    <h3 class="title">{{title}}</h3>
                </div>
                <form role="form" @submit.prevent="validateBeforeSubmit">
                    <p class="text-message" v-if="author">Author: {{author}}</p>
                    <p class="text-danger" v-if="errorMessage">{{errorMessage}}</p>
                    <div class="form-group"> 
                        <label class="control-label" for="pNote">Note</label> 
                        <input v-model="payment.note" v-validate="'max:250'"type="text" class="form-control boxed" id="pNote" placeholder="Note" Name="Note">
                        <span class="text-danger" v-if="errors.has('Note')">{{ errors.first('Note') }}</span>
                    </div>
                    <div class="form-group"> 
                        <label class="control-label" for="pAmount">Amount</label> 
                        <input v-model="payment.amount" v-validate="'required|decimal'" type="text" class="form-control boxed" id="pAmount" placeholder="Amount" Name="Amount">  
                        <span class="text-danger" v-if="errors.has('Amount')">{{ errors.first('Amount') }}</span>
                    </div>
                    <div class="form-group"> 
                        <label class="control-label" for="pDate">Payment Date</label> 
                        <input v-model="payment.date" v-validate="'date_format:L'"  type="text" class="form-control boxed" id="pDate" placeholder="Date" Name="Date">  
                        <span class="text-danger" v-if="errors.has('Date')">{{ errors.first('Date') }}</span>
                    </div>
                    <div class="form-group"> 
                        <label class="control-label" for="pCategory">Category</label> 
                        <select class="form-control boxed" id="pCategory" placeholder="Category"  v-model="payment.categoryId" v-validate="'required'" Name="Category">  
                            <option :value="undefined">Select one</option>
                            <option 
                                v-for="(category,index) in categories"
                                v-bind:key="index"  
                                :value="category.id">{{category.name}}
                            </option>
                        </select>
                        <span class="text-danger" v-if="errors.has('Category')">{{ errors.first('Category') }}</span>
                    </div>
                    <button class="btn btn-primary" type="Submit">{{buttontest}}</button> <router-link :to="{ name: 'group', params: { groupId: this.$route.params.groupId }}" class="btn btn-primary">Cancel</router-link>
                </form>
            </div>
        </div>
    </div>
</template>
<script>
import $ from 'jquery'
export default {
    props:['paymentId'],
    data:function() {
        return{
            payment:{
                id:Number,
                note:'',
                amount: '',
                categoryId: Number,
                date: '',
                groupId: this.$route.params.groupId,
                user:{firstName:'',lastName:''}
            },
            categories:[],
            actionlink:'',
            buttontest:'',
            title:'',
            errorMessage:'',
            mounted:false
        }
    },
    computed:{
        author: function(){
            if (!!this.payment.user.firstName || !!this.payment.user.lastName){
                return this.payment.user.firstName + ' ' + this.payment.user.lastName;
            }else{
                return '';
            }
        }
    },
    methods:{
        init:function(){
            var vm = this;
            vm.fetchCategories();
            if (vm.paymentId){
                vm.getPayment();
                vm.actionlink = vm.$apiHelper.editPaymentUrl(vm.paymentId);
                vm.buttontest = 'Save';
                vm.title = 'Edit payment';
            }else{
                vm.payment.note = '';
                vm.payment.amount = '';
                vm.payment.user.firstName = '';
                vm.payment.user.lastName ='';
                vm.payment.categoryId = undefined;
                vm.actionlink = vm.$apiHelper.paymentBaseUrl;
                vm.buttontest = 'Add';
                vm.title = 'Add payment';
                vm.payment.date = this.$moment().format('L')
            }
        },
        getPayment: function(){
            var vm = this;
            vm.errorMessage = '';
            vm.$http.get(vm.$apiHelper.getPaymentUrl(vm.paymentId))
            .then(function(response){
                vm.payment = response.data;
                vm.payment.date = vm.$moment(vm.payment.date, vm.$appConfig.dateTimeFormat).format('L');
                vm.payment.categoryId = response.data.category.id;
            }).catch(function(ex){
                vm.errorMessage = "Something went wrong: "+ ex;
            });
        },
        processPayment: function(){
            var vm = this;
            vm.errorMessage = '';
            vm.$http.post(vm.actionlink, vm.payment)
            .then(function(response){
                vm.$router.push({ name: 'group', params: { groupId: vm.$route.params.groupId }});
            }).catch(function(ex){
                vm.errorMessage = "Something went wrong: "+ ex;
            });
        },
        fetchCategories: function(){
            var vm = this;
            vm.$http.get(vm.$apiHelper.categoryBaseUrl)
            .then(function (response) {
                vm.categories = response.data;
            })
            .catch(function (error) {
                errorMessage= 'Error while loading categories: ' + error;    
            });
        },
        validateBeforeSubmit(e) {
            this.$validator.validateAll();
            if (!this.errors.any()) {
                this.processPayment();
            }
        }
        
    },
    created:function(){
        this.init(); 
    },
    mounted(){
        $("#pDate").datepicker().on(
             "changeDate", () => {this.payment.date = $('#pDate').val()});
        this.mounted = true;
    },
    watch:{
        '$route':'init'
    }
}
</script>

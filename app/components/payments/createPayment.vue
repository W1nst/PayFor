<template>
    <div class="card-header bordered">
        <div class="col-md-12">
            <div class="card card-block">
                <div class="title-block">
                    <h3 class="title">Payment {{paymentId}}</h3>
                </div>
                <form role="form" @submit.prevent="validateBeforeSubmit">
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
                        <input v-model="payment.date" v-validate="'date_format:MM/DD/YYYY'"  type="text" class="form-control boxed" id="pDate" placeholder="Date" Name="Date">  
                        <span class="text-danger" v-if="errors.has('Date')">{{ errors.first('Date') }}</span>
                    </div>
                    <div class="form-group"> 
                        <label class="control-label" for="pCategory">Category</label> 
                        <select class="form-control boxed" id="pCategory" placeholder="Category"  v-model="payment.categoryId" v-validate="'required'" Name="Category">  
                            <option 
                                v-for="(category,index) in categories"
                                v-bind:key="index"  
                                :value="category.id">{{category.name}}
                            </option>
                        </select>
                        <span class="text-danger" v-if="errors.has('Category')">{{ errors.first('Category') }}</span>
                    </div>
                    <button class="btn btn-primary" type="Submit">{{buttontest}}</button>
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
                date: this.$moment().format('L'),
                groupId: this.$route.params.groupId
            },
            categories:[],
            actionlink:'/api/payment/',
            buttontest:'Add',
            errorMessage:''
        }
    },
    methods:{
        getCategories: function(){
            var vm = this;
            vm.$http.get('/api/category/')
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
                    this.createPayment();
                }
        },
        createPayment: function(){
            var vm = this;
            vm.errorMessage = '';
            vm.$http.post(vm.actionlink, vm.payment)
            .then(function(response){
                vm.$router.push({ name: 'group', params: { groupId: vm.$route.params.groupId }});
            }).catch(function(ex){
                vm.errorMessage = "Something went wrong: "+ ex + " - " + ex.response.data.name;
            });
        },
        getPayment: function(){
            if (!this.paymentId) return;
            var vm = this;
            vm.errorMessage = '';
            vm.$http.get('/api/payment/'+vm.paymentId)
            .then(function(response){
                vm.payment = response.data;
                vm.payment.date = vm.$moment(vm.payment.date, 'YYYY-MM-DD HH:mm:ss').format('L');
                vm.payment.categoryId = response.data.category.id;
                vm.actionlink = '/api/payment/'+vm.paymentId+'/edit';
                vm.buttontest = 'Save';
            }).catch(function(ex){
                vm.errorMessage = "Something went wrong: "+ ex + " - " + ex.response.data.name;
            });
        }
    },
    created:function(){
        this.getCategories();
        this.getPayment(); 
    },
    mounted(){
        $("#pDate").datepicker().on(
     		"changeDate", () => {this.payment.date = $('#pDate').val()});
    },
    watch:{
        '$route':'getCategories',
        '$route':'getPayment'
    }
}
</script>

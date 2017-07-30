<template>
    <div class="col-md-12">
        <div class="card card-block">
            <div class="title-block">
                <h3 class="title">Payment</h3>
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
                <button class="btn btn-primary" type="Submit">Add</button>
            </form>
        </div>
    </div>
</template>
<script>
import axios from 'axios';
export default {
    data:function() {
        return{
            payment:{
                note:'',
                amount: '',
                categoryId: Number,
                groupId: this.$route.params.groupId
            },
            categories:[],
            errorMessage:''
        }
    },
    methods:{
        getCategories: function(){
            var vm = this;
            axios.get('/api/category/')
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
            axios.post('/api/payment/',vm.payment)
            .then(function(response){
                vm.$router.push({ name: 'group', params: { groupId: vm.$route.params.groupId }});
            }).catch(function(ex){
                vm.errorMessage = "Something went wrong: "+ ex + " - " + ex.response.data.name;
            });
        }
    },
    created:function(){
        this.getCategories();
    }
}
</script>

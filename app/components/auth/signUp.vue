<template>
    <div class="auth">
        <div class="auth-container">
            <div class="card">
                <header class="auth-header">
                    <h1 class="auth-title"><div class="logo"><span class="l l1"></span> <span class="l l2"></span> <span class="l l3"></span> <span class="l l4"></span> <span class="l l5"></span> </div> PayFor </h1>
                </header>
                <div class="auth-content">
                    <p class="text-xs-center">SIGNUP TO GET INSTANT ACCESS</p>
                    <form id="signup-form" @submit.prevent="validateBeforeSubmit">
                        <p class="text-danger" v-if="errorMessage">{{errorMessage}}</p>
                        <div class="form-group"> 
                            <label for="firstname">Name</label>
                            <div class="row">
                                <div class="col-sm-6"> 
                                    <input type="text" class="form-control underlined" v-model="signUpModel.firstName" name="firstname"  v-validate="'required'" id="firstname" placeholder="Enter firstname"> 
                                    <span class="text-danger" v-if="errors.has('firstname')">{{ errors.first('firstname') }}</span>
                                </div>
                                <div class="col-sm-6"> 
                                    <input type="text" class="form-control underlined" v-model="signUpModel.lastName" name="lastname" id="lastname" placeholder="Enter lastname"> 
                                </div>
                            </div>
                        </div>
                        <div class="form-group"> 
                            <label for="email">Email</label> 
                            <input type="email" class="form-control underlined" v-model="signUpModel.email" name="email" id="email" v-validate="'required|email'" placeholder="Enter email address"> 
                            <span class="text-danger" v-if="errors.has('email')">{{ errors.first('email') }}</span>
                        </div>
                        <div class="form-group"> <label for="password">Password</label>
                            <div class="row">
                                <div class="col-sm-6"> 
                                    <input type="password" class="form-control underlined" v-model="signUpModel.password" name="password" id="password" v-validate="'required'" placeholder="Enter password"> 
                                    <span class="text-danger" v-if="errors.has('password')">{{ errors.first('password') }}</span>
                                </div>
                                <div class="col-sm-6"> 
                                    <input type="password" class="form-control underlined" v-model="signUpModel.retype_password" name="retype_password" id="retype_password" v-validate="'required|confirmed:password'" placeholder="Re-type password"> 
                                    <span class="text-danger" v-if="errors.has('retype_password')">{{ errors.first('retype_password') }}</span>
                                </div>
                            </div>
                        </div>
                        <div class="form-group"> 
                            <label for="agree">
                                <input class="checkbox" name="agree" id="agree" type="checkbox" v-validate="'required'"> 
                                <span>Agree the terms and <a href="#">policy</a></span>
                                <span class="text-danger" v-if="errors.has('agree')">{{ errors.first('agree') }}</span>
                            </label> 
                        </div>
                        <div class="form-group"> <button type="submit" class="btn btn-block btn-primary">Sign Up</button> </div>
                        <div class="form-group">
                            <p class="text-muted text-xs-center">Already have an account? <router-link :to="{ name: 'login'}">Login!</router-link></p>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
export default {
    data(){
        return {
            signUpModel:{
                firstName:'',
                lastName:'',
                email:'',
                password:'',
                retype_password:''
            },
            errorMessage:''
        }
    },
    methods:{
        signUp: function(){
            var vm = this;
            vm.$auth.signUp(vm.signUpModel).then(
                function(result) {
                    vm.$router.push({ name: 'groups'});
                },
                function(error){
                    vm.errorMessage = error.response.data.message;
                }   
            );
        },
        validateBeforeSubmit(e) {
            this.$validator.validateAll();
            if (!this.errors.any()) {
                this.signUp();
            }
        }
    }
    
}
</script>


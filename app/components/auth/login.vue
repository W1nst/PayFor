<template>
    <div class="auth">
            <div class="auth-container">
                <div class="card">
                    <header class="auth-header">
                        <h1 class="auth-title">
                            <div class="logo"> <span class="l l1"></span> <span class="l l2"></span> <span class="l l3"></span> <span class="l l4"></span> <span class="l l5"></span> </div> ModularAdmin </h1>
                    </header>
                    <div class="auth-content">
                        <p class="text-xs-center">LOGIN TO CONTINUE</p>
                        <form role="form" @submit.prevent="validateBeforeSubmit">
                            <p class="text-danger" v-if="errorMessage">{{errorMessage}}</p>
                            <div class="form-group"> 
                                <label for="username">Email</label> 
                                <input type="email" class="form-control underlined" v-validate="'required'" v-model="loginModel.email" name="Email" id="pEmail" placeholder="Your email address" required="" aria-required="true"> 
                                <span class="text-danger" v-if="errors.has('Email')">{{ errors.first('Email') }}</span>
                            </div>
                            <div class="form-group"> 
                                <label for="password">Password</label> 
                                <input type="password" class="form-control underlined" v-model="loginModel.password" name="Password" id="pPassword" placeholder="Your password" required="" aria-required="true"> </div>
                                <span class="text-danger" v-if="errors.has('Password')">{{ errors.first('Password') }}</span>
                            <div class="form-group"> <label for="remember">
            <input class="checkbox" id="remember" type="checkbox"> 
            <span>Remember me</span>
          </label> <a href="reset.html" class="forgot-btn pull-right">Forgot password?</a> </div>
                            <div class="form-group"> <button type="submit" class="btn btn-block btn-primary">Login</button> </div>
                            <div class="form-group">
                                <p class="text-muted text-xs-center">Do not have an account? <a href="signup.html">Sign Up!</a></p>
                            </div>
                        </form>
                    </div>
                </div>
                <div class="text-xs-center"> <a href="index.html" class="btn btn-secondary rounded btn-sm">
      <i class="fa fa-arrow-left"></i> Back to dashboard
    </a> </div>
            </div>
    </div>
</template>
<script>
export default {
    data() {
        return {
            loginModel:{
                email: '',
                password: ''    
            },
            errorMessage:''
        }
    },
    methods:{
        login: function(){
            var vm = this;
            vm.$auth.login(vm.loginModel).then(function(result) {
                if (result){
                    vm.$router.push({ name: 'groups'});
                }else{
                    vm.errorMessage = 'Incorrect email or password!';
                }
            });
            
        },
        validateBeforeSubmit(e) {
            this.$validator.validateAll();
            if (!this.errors.any()) {
                this.login();
            }
        }
    }
}
</script>

<template>
    <div class="col-md-12">
        <div class="card card-block sameheight-item">
            <div class="title-block">
                <h3 class="title">Group</h3>
            </div>
            <form role="form" @submit.prevent="validateBeforeSubmit">
                <p class="text-danger" v-if="errorMessage">{{errorMessage}}</p>
                <div class="form-group" :class="{'has-error': errors.has('Name'), 'has-success': !errors.has('Name') }">
                    <label class="control-label" for="gName">Name</label>
                    <input 
                        v-model="newGroup.name"  
                        v-validate="'required|min:3'" 
                        class="form-control boxed" 
                        type="text" 
                        placeholder="Group Name" 
                        id="gName" name="Name">
                    <span class="text-danger" v-if="errors.has('Name')">{{ errors.first('Name') }}</span>
                </div>
                <div class="form-group"> 
                    <button class="btn btn-primary" type="Submit">Create</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    export default  {
        data:function() {
            return{
                newGroup:{
                    name:''
                },
                errorMessage:''
            }
        },
        methods:{
            createGroup : function (){
                var vm = this;
                vm.isBusy = true;
                vm.errorMessage = '';
                vm.$http.post(vm.$apiHelper.groupBaseUrl, vm.newGroup)
                .then(function(response){
                    vm.$router.push('groups');
                }).catch(function(error){
                    vm.errorMessage = "Something went wrong:  " + error.response.data.message;
                });
            },
            validateBeforeSubmit(e) {
                this.$validator.validateAll();
                if (!this.errors.any()) {
                    this.createGroup();
                }
            }
        }
    }
</script>
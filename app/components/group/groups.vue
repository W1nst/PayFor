<template>
    <section class="section">
        <div class="row">
            <div class="card ">
                <is-busy v-bind:is-busy="isBusy"></is-busy>
                <div v-if="!isBusy" class="content table-responsive table-full-width">
                    <table class="table table-hover table-striped">
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Create Date</th>
                                <th>Author</th>
                                <th>Delete</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr is="grouprow" 
                                v-for="(group,index) in groups" 
                                v-bind:group="group" 
                                v-bind:key="index" 
                                v-on:remove="deleteGroup(index)"
                                v-on:showgroup="showGroup(group.id)">
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <creategroup v-bind:group="newGroup" v-bind:error-message="errorMessage" v-on:create="createGroup()"></creategroup>
        </div>
    </section>
</template>
<script>
    import axios from 'axios';

    export default {
        data:function (){
            return{
                isBusy: false,
                newGroup:{
                    name:''
                },
                groups:[],
                errorMessage:''
            }
        },
        methods: {
            deleteGroup:function(index){
                if (!confirm('Are you sure you want to delete "'+ this.groups[index].name+'" thing from the database?')) return;
                var vm = this;
                vm.errorMessage = "";
                axios.post('/api/group/'+vm.groups[index].id+'/delete')
                .then(function(response){
                    vm.groups.splice(index, 1);
                }).catch(function(ex){
                    vm.errorMessage = "Something went wrong: "+ ex;
                });       
            },
            createGroup : function (){
                var vm = this;
                vm.isBusy = true;
                vm.errorMessage = "";
                axios.post('/api/group/',vm.newGroup)
                .then(function(response){
                    vm.groups.push(response.data);
                    vm.newGroup={};
                    vm.isBusy = false;
                }).catch(function(ex){
                    vm.errorMessage = "Something went wrong: "+ ex;
                    vm.isBusy = false;
                });
                
            },
            fetchGroups: function(){
                var vm = this;
                vm.isBusy = true;
                axios.get(`/api/group/`)
                    .then(function (response) {
                        vm.groups = response.data;
                        vm.isBusy = false;  
                    })
                    .catch(function (error) {
                        console.log(error);
                        vm.isBusy = false;      
                    });
            } ,
            showGroup: function(id){
                this.$emit('showgroup',id);
            }
        },
        created:function(){
            this.fetchGroups();
        }
    }
</script>
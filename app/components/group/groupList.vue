<template>
    <div class="items-list-page">
        <div class="card items">
            <ul class="item-list striped">
                <li class="item item-list-header hidden-sm-down">
                    <div class="item-row">
                        <div class="item-col item-col-header item-col-title">
                            <div> <span>Name</span> </div>
                        </div>
                        <div class="item-col item-col-header item-col-author">
                            <div class="no-overflow"> <span>Author</span> </div>
                        </div>
                        <div class="item-col item-col-header item-col-date">
                            <div> <span>Created</span> </div>
                        </div>
                        <div class="item-col item-col-header fixed item-col-actions-dropdown" v-if="editable"> </div>
                    </div>
                </li>
                <li is="grouprow" 
                    v-for="(group,index) in groups" 
                    v-bind:group="group" 
                    v-bind:key="index" 
                    v-on:remove="deleteGroup(index)"
                    v-bind:editable="editable">
                </li>
            </ul>
        </div>
        <nav class="text-xs-right">
            <ul class="pagination">
                <li class="page-item"> <a class="page-link" href=""> Prev </a></li>
                <li class="page-item active"> <a class="page-link" href="">1</a></li>
                <li class="page-item"> <a class="page-link" href="">2</a></li>
                <li class="page-item"> <a class="page-link" href="">3</a></li>
                <li class="page-item"> <a class="page-link" href="">4</a></li>
                <li class="page-item"> <a class="page-link" href="">5</a></li>
                <li class="page-item"> <a class="page-link" href="">Next</a></li>
            </ul>
        </nav>
    </div>
</template>
<script>
    export default {
        props:['usergroups','editable'],
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
            init: function(){
                var vm = this;
                if (vm.usergroups){
                    vm.groups = vm.usergroups;
                }else{
                    vm.fetchGroups();
                }
            },
            deleteGroup:function(index){
                //if (!confirm('Are you sure you want to delete "'+ this.groups[index].name+'" thing from the database?')) return;
                var vm = this;
                vm.$swal({
                        title: "Are you sure?",
                        text: "You will not be able to recover this group: " + vm.groups[index].name + "!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes, delete it!",
                        closeOnConfirm: false,
                        showLoaderOnConfirm: true,
                    },
                    function(){
                        vm.errorMessage = "";
                        vm.$http.post(vm.$apiHelper.deleteGroupUrl(vm.groups[index].id))
                        .then(function(response){
                            vm.groups.splice(index, 1);
                            swal("Deleted!", "Your group has been deleted.", "success");
                        }).catch(function(error){
                            console.log(error.response.data.message);
                            vm.errorMessage =  error.response.data.message;
                            swal("Error", vm.errorMessage, "error");
                        }); 
                });
                      
            },
            fetchGroups: function(){
                var vm = this;
                vm.isBusy = true;
                vm.$http.get(vm.$apiHelper.groupBaseUrl)
                    .then(function (response) {
                        vm.groups = response.data;
                        vm.isBusy = false;  
                    })
                    .catch(function (error) {
                        console.log(error.response.data.message);
                        vm.isBusy = false;      
                    });
            }
        },
        created:function(){
            this.init();
        }
    }
</script>
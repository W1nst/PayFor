<template>
    <article class="content items-list-page">
        <div class="title-search-block">
            <div class="title-block">
                <div class="row">
                    <div class="col-md-6">
                        <h3 class="title"> 
                            Groups 
                            <router-link to="/creategroup" class="btn btn-primary btn-sm rounded-s">Add New</router-link>
                            <div class="action dropdown"> 
                                <button class="btn  btn-sm rounded-s btn-secondary dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    More actions...
                                </button>
                                <div class="dropdown-menu" aria-labelledby="dropdownMenu1"> 
                                    <a class="dropdown-item" href="#"><i class="fa fa-pencil-square-o icon"></i>Mark as a draft</a> 
                                    <a class="dropdown-item" href="#" data-toggle="modal" data-target="#confirm-modal"><i class="fa fa-close icon"></i>Delete</a>                                                </div>
                            </div>
                        </h3>
                        <p class="title-description"> List of all groups you are in </p>
                    </div>
                </div>
            </div>
            <div class="items-search">
                <form class="form-inline">
                    <div class="input-group"> <input type="text" class="form-control boxed rounded-s" placeholder="Search for..."> 
                        <span class="input-group-btn">
                            <button class="btn btn-secondary rounded-s" type="button">
                                <i class="fa fa-search"></i>
                            </button>
                        </span> 
                    </div>
                </form>
            </div>
        </div>
        <grouplist v-bind:editable="true"></grouplist>
        
    </article>
</template>
<script>
    export default {
        // props:['userGroups'],
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
        // methods: {
        //     init: function(){
        //         var vm = this;
        //         if (vm.userGroups){
        //             vm.groups = vm.userGroups;
        //         }else{
        //             vm.fetchGroups();
        //         }
        //     },
        //     deleteGroup:function(index){
        //         //if (!confirm('Are you sure you want to delete "'+ this.groups[index].name+'" thing from the database?')) return;
        //         var vm = this;
        //         vm.$swal({
        //                 title: "Are you sure?",
        //                 text: "You will not be able to recover this group: " + vm.groups[index].name + "!",
        //                 type: "warning",
        //                 showCancelButton: true,
        //                 confirmButtonColor: "#DD6B55",
        //                 confirmButtonText: "Yes, delete it!",
        //                 closeOnConfirm: false,
        //                 showLoaderOnConfirm: true,
        //             },
        //             function(){
        //                 vm.errorMessage = "";
        //                 vm.$http.post(vm.$apiHelper.deleteGroupUrl(vm.groups[index].id))
        //                 .then(function(response){
        //                     vm.groups.splice(index, 1);
        //                     swal("Deleted!", "Your group has been deleted.", "success");
        //                 }).catch(function(error){
        //                     console.log(error.response.data.message);
        //                     vm.errorMessage =  error.response.data.message;
        //                     swal("Error", vm.errorMessage, "error");
        //                 }); 
        //         });
                      
        //     },
        //     fetchGroups: function(){
        //         var vm = this;
        //         vm.isBusy = true;
        //         vm.$http.get(vm.$apiHelper.groupBaseUrl)
        //             .then(function (response) {
        //                 vm.groups = response.data;
        //                 vm.isBusy = false;  
        //             })
        //             .catch(function (error) {
        //                 console.log(error.response.data.message);
        //                 vm.isBusy = false;      
        //             });
        //     }
        // },
        // created:function(){
        //     this.init();
        // }
    }
</script>
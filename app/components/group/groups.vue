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
                        <div class="item-col item-col-header fixed item-col-actions-dropdown"> </div>
                    </div>
                </li>
                <li is="grouprow" 
                    v-for="(group,index) in groups" 
                    v-bind:group="group" 
                    v-bind:key="index" 
                    v-on:remove="deleteGroup(index)">
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
    </article>
</template>
<script>
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
            this.fetchGroups();
        }
    }
</script>
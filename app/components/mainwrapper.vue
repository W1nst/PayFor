<template>
    <div>
        <sidebar></sidebar>
        <!-- <router-view></router-view> -->
        <article class="content">
            <component 
                v-on:showgroup = "showgroup" 
                v-on:showallgroups = "showallgroups"
                v-bind:is = "currentView"
                v-bind:group = "group">
            </component>
        </article>
    </div>
</template>

<script>
import axios from 'axios';

export default  {
    data:function(){
        return{
            currentView:"groups",
            group:{}
        }
    },
    methods: {
        showgroup: function(id){
            var vm = this;
            vm.isBusy = true;
            axios.get(`/api/group/`+id)
                .then(function (response) {
                    vm.group = response.data;
                    vm.currentView = "group";
                })
                .catch(function (error) {
                    console.log(error);    
                });
        },
        showallgroups: function(){
            this.currentView = "groups";
        }

    }
}
</script>
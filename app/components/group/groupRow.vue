<template>

    <router-link :to="{ name: 'group', params: { groupId: group.id }}" tag="li" class="item">
        <div class="item-row">
            <div class="item-col fixed pull-left item-col-title">
                <div class="item-heading">Name</div>
                <div>
                    <a href="item-editor.html" class="">
                        <h4 class="item-title"> {{group.name}} </h4>
                    </a>
                </div>
            </div>
            <div class="item-col item-col-author">
                <div class="item-heading">Author</div>
                <div class="no-overflow"> <a href="">{{authorName}}</a> </div>
            </div>
            <div class="item-col item-col-date">
                <div class="item-heading">Created</div>
                <div class="no-overflow"> {{group.createDateTime | moment('LLL')}} </div>
            </div>
            <div class="item-col fixed item-col-actions-dropdown">
                <div class="item-actions-dropdown"> 
                    <a class="item-actions-toggle-btn">
                        <span class="inactive">
                            <i class="fa fa-trash-o" v-on:click.stop="$emit('remove')"></i>
                        </span>
                    </a>
                </div>
            </div>
        </div>
    </router-link>    
</template>
<script>
    export default {
        props: ['group']  ,
        computed:{
            authorName: function(){
                if (this.group.authorUser){
                    return this.group.authorUser.firstName + ' ' +  this.group.authorUser.lastName;
                } else {
                    return '';
                } 
            }
        }      
    }
</script>


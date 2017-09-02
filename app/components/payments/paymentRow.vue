<template>
    <!-- <li> -->
        <router-link :to="{ name: 'editpayment', params: { paymentId: payment.id }}" tag="li" active-class="item-active" >
        <div class="item-row">
            <div class="item-col item-col-title no-overflow">
                <div>
                    <h4 class="item-title no-wrap"> {{payment.note}}</h4>
                </div>
            </div>
            <div class="item-col item-col-sales">
                <div class="item-heading">Amount</div>
                <div> {{payment.amount}} </div>
            </div>
            <div class="item-col item-col-category">
                <div class="item-heading">Category</div>
                <div class="no-overflow">
                    <div>{{payment.category.name}}</div>
                </div>
            </div>
            <div class="item-col item-col-author">
                <div class="item-heading">Author</div>
                <div class="no-overflow">{{authorName}}</div>
            </div>
            <div class="item-col item-col-date">
                <div class="item-heading">Payment date</div>
                <div> {{payment.date | moment('L')}} </div>
            </div>
            <div class="item-col fixed item-col-actions-dropdown">
                <div class="item-actions-dropdown"> 
                    <!-- <a class="item-actions-toggle-btn"> -->
                        <span class="active">
                            <i class="fa fa-trash-o" v-on:click.stop="$emit('remove')"></i>
                        </span>
                    <!-- </a> -->
                </div>
            </div>
        </div>
        </router-link>
    <!-- </li> -->
</template>
<script>
    export default {
        props:["payment"],
        computed:{
            authorName: function(){
                if (this.payment.user){
                    return this.payment.user.firstName + ' ' +  this.payment.user.lastName;
                };
            }
        }
    }
</script>


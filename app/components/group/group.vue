<template>
    <div class="col-md-8">
        <button class="btn btn-info btn-fill" v-on:click="$emit('showallgroups')">Back</button>
        <div class="card ">
            <div class="header">
                <h4 class="title">{{group.name}}</h4>
                <p class="category" v-show="group.authorName">Author: {{group.authorName}}</p>
            </div>
            <div class="content">
                <div class="table-full-width">
                    <table class="table">
                        <tbody>
                            <tr v-for="(payment,index) in group.payments" 
                                v-bind:key="index">
                                <td>
                                       {{payment.amount}}
                                </td>
                                <td>{{payment.category.name}}</td>
                                <td>{{payment.addUser}}</td>
                                <td class="td-actions text-right">
                                    <button type="button" rel="tooltip" title="" class="btn btn-info btn-simple btn-xs" data-original-title="Edit Task">
                                        <i class="fa fa-edit"></i>
                                    </button>
                                    <button type="button" rel="tooltip" title="" class="btn btn-danger btn-simple btn-xs" data-original-title="Remove">
                                        <i class="fa fa-times"></i>
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div class="footer">
                    <hr>
                    <div class="header">
                        Total:{{total}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props : ['group'],
        data () {
            return {
                
            }
        },
        computed: {
            total: function(){
                return this.group.payments.reduce(function(prev, payment){
                    return prev + payment.amount; 
                },0);
            }   
        },
        methods: {
            
        }
    }
</script>
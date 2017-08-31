//api
export default {
    //group
    groupBaseUrl: '/api/group/',
    getGroupUrl : function (id){
        return this.groupBaseUrl + id;
    },
    deleteGroupUrl : function (id){
        return this.groupBaseUrl + id + '/delete';
    },
    //payment
    paymentBaseUrl: '/api/payment/',
    getPaymentUrl : function (id){
        return this.paymentBaseUrl + id;
    },
    editPaymentUrl : function (id){
        return this.paymentBaseUrl + id + '/edit';
    },
    deletePaymentUrl : function (id){
        return this.paymentBaseUrl + id + '/delete';
    },

    //categories
    categoryBaseUrl : '/api/category/',
    //auth
    authBaseUrl : '/auth/',
    generateTokenUrl : function(){
        return this.authBaseUrl + 'GenerateToken';
    },
    signUpUrl : function(){
        return this.authBaseUrl + 'SignUp';
    }
}
<template>
    <div>
        <sidebar></sidebar>
        <router-view class="content"></router-view> 
    </div>
</template>

<script>
export default  {
    name:'main',
    data:function(){
        return{
        }
    }
}
</script>
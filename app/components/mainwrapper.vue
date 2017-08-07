<template>
    <div>
        <sidebar></sidebar>
        <transition name="component-fade" mode="out-in">
            <router-view class="content"></router-view> 
        </transition>
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
<style>
.component-fade-enter-active, .component-fade-leave-active {
  transition: opacity .3s ease;
}
.component-fade-enter, .component-fade-leave-to
/* .component-fade-leave-active до версии 2.1.8 */ {
  opacity: 0;
}
</style>

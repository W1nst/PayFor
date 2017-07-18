<template>
    <tr v-on:click="$emit('showgroup')">
        <td>{{group.name}}</td>
        <td>{{group.createDateTime | moment('LLL')}}</td>
        <td>{{group.authorName}}</td>
        <td><button v-on:click.stop="$emit('remove')"><i class='pe-7s-trash'></i></button></td>
    </tr>
</template>
<script>
    export default {
        props: ['group']        
    }
</script>


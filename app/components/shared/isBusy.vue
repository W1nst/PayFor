<template>
    <div v-if="isBusy" class="center-block text-center">
        <i class="fa fa-circle-o-notch fa-spin fa-3x fa-fw"></i>
        <span class="sr-only">Loading...</span>
    </div>
</template>
<script>
module.exports = {
    props: ['isBusy']
}
</script>
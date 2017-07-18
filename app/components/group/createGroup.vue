<template>
    <div class="row">
        <div class="col-md-12">
            <div class="card">
                <div class="header">
                    <h4 class="title">Create Group</h4>
                </div>
                <div class="content">
                    <p class="text-danger" v-if="errorMessage">{{errorMessage}}</p>
                    
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Name</label>
                                    <input type="text" class="form-control" placeholder="Group Name" value="" v-model="group.name">
                                </div>
                            </div>
                        </div>
                        <button v-on:click="$emit('create')" class="btn btn-info btn-fill pull-right">Create</button>
                        <div class="clearfix"></div>
                    
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default  {
        props: {
            group:{
                name:String
            },
            errorMessage:''
        }
    }
</script>
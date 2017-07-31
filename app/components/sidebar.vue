<template>
    <aside class="sidebar">
        <div class="sidebar-container">
            <div class="sidebar-header">
                <div class="brand">
                    <div class="logo"> 
                        <span class="l l1"></span> 
                        <span class="l l2"></span> 
                        <span class="l l3"></span> 
                        <span class="l l4"></span> 
                        <span class="l l5"></span> 
                    </div> 
                    PayFor
                </div>
            </div>
            <nav class="menu">
                <ul class="nav metismenu" id="sidebar-menu">
                    <li> 
                        <a href="index.html"><i class="fa fa-home"></i> Dashboard</a> 
                    </li>
                    <router-link to="/groups" active-class="active" tag="li"><a>Groups list</a></router-link>
                    <router-link to="/creategroup" active-class="active" tag="li" ><a>Create group</a></router-link>
                </ul>
            </nav>
        </div>
        <footer class="sidebar-footer">
            <ul class="nav metismenu" id="customize-menu">
                <li>
                    <a href=""><i class="fa fa-cog"></i>Delete</a> 
                </li>
            </ul>
        </footer>
    </aside> 
</template>

<script>
export default {
    data:function(){
        return{
            
        }
    }
}
</script>

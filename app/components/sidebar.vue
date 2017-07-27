<template>
    <aside class="sidebar">
        <div class="sidebar-container">
            <div class="sidebar-header">
                <div class="brand">
                    <div class="logo"> <span class="l l1"></span> <span class="l l2"></span> <span class="l l3"></span> <span class="l l4"></span> <span class="l l5"></span> </div> Modular Admin </div>
            </div>
            <nav class="menu">
                <ul class="nav metismenu" id="sidebar-menu">
                    <li class="active"> 
                        <a href="index.html">
                            <i class="fa fa-home"></i> Dashboard
                        </a> 
                    </li>
                    <li> 
                        <a href="">
                            <i class="fa fa-th-large"></i> Groups <i class="fa arrow"></i>
                        </a>
                        <ul>
                            <li> 
                                <router-link to="/groups"> Groups list </router-link>
                            </li>
                            <li> 
                                <router-link to="/creategroup"> Create group </router-link>
                            </li>
                        </ul>
                    </li>
                </ul>
            </nav>
        </div>
        <footer class="sidebar-footer">
            <ul class="nav metismenu" id="customize-menu">
                <li>
                    <a href="">
                        <i class="fa fa-cog"></i> Customize
                    </a> 
                </li>
            </ul>
        </footer>
    </aside> 
</template>

<script>
export default {
    data:function(){
        return{
            test: 200
        }
    }
}
</script>

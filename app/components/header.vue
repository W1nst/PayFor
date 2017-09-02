<template>
    <header class="header">
        <div class="header-block header-block-collapse hidden-lg-up"> 
            <button class="collapse-btn" id="sidebar-collapse-btn"><i class="fa fa-bars"></i></button> 
        </div>
        <div class="header-block header-block-search hidden-sm-down">
            <form role="search">
                <div class="input-container"> <i class="fa fa-search"></i> <input type="search" placeholder="Search">
                    <div class="underline"></div>
                </div>
            </form>
        </div>
        <div class="header-block header-block-nav">
            <ul class="nav-profile">
                <li class="notifications new"> 
                    <a href="" data-toggle="dropdown">
                        <i class="fa fa-bell-o"></i>
                        <sup>
                            <span class="counter">8</span>
                        </sup>
                    </a>
                    <div class="dropdown-menu notifications-dropdown-menu">
                        <ul class="notifications-container">
                            <li>
                                <a href="" class="notification-item">
                                    <div class="img-col">
                                        <div class="img" style="background-image: url('assets/faces/3.jpg')"></div>
                                    </div>
                                    <div class="body-col">
                                        <p> <span class="accent">Zack Alien</span> pushed new commit: <span class="accent">Fix page load performance issue</span>. </p>
                                    </div>
                                </a>
                            </li>
                            <li>
                                <a href="" class="notification-item">
                                    <div class="img-col">
                                        <div class="img" style="background-image: url('assets/faces/5.jpg')"></div>
                                    </div>
                                    <div class="body-col">
                                        <p> <span class="accent">Amaya Hatsumi</span> started new task: <span class="accent">Dashboard UI design.</span>. </p>
                                    </div>
                                </a>
                            </li>
                            <li>
                                <a href="" class="notification-item">
                                    <div class="img-col">
                                        <div class="img" style="background-image: url('assets/faces/8.jpg')"></div>
                                    </div>
                                    <div class="body-col">
                                        <p> <span class="accent">Andy Nouman</span> deployed new version of <span class="accent">NodeJS REST Api V3</span> </p>
                                    </div>
                                </a>
                            </li>
                        </ul>
                        <footer>
                            <ul>
                                <li> <a href="">View All</a> </li>
                            </ul>
                        </footer>
                    </div>
                </li>
                <router-link :to="{ name: 'user'}" tag="li" class="profile dropdown">
                    <div class="img" style="background-image: url('https://avatars3.githubusercontent.com/u/3959008?v=3&amp;s=40')"> </div> 
                    <span class="name">{{userName}}</span> 
                </router-link>
                <li>
                    <span @click="logout()"  class="btn btn-sm header-btn">
                        <i class="fa fa-sign-out"></i>
                        <span>Logout</span>
                    </span> 
                </li>
            </ul>
            
        </div>
        <!-- <div class="header-block header-block-buttons"> 
            <span @click="logout()"  class="btn btn-sm header-btn">
                <i class="fa fa-sign-out"></i>
                <span>Logout</span>
            </span> 
        </div> -->
    </header>
</template>
<script>
export default {
    data:function(){
        return {
        }
    },
    computed: {
        userName:function(){
            return this.$auth.firstName + " " + this.$auth.lastName;
        }
    },
    methods:{
        logout: function(){
            var vm = this;
            vm.$auth.logout();
        }
    }
}
</script>


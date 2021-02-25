// import React, { Component } from 'react';
// import { NavLink } from 'react-router-dom';
// class Navbar extends Component {
//     render(){
//         return(
//             <nav className="navBar">
//                 <ul>
//                     <li><NavLink exact to="/">Home</NavLink></li>
//                     <li><NavLink to="/about/">About</NavLink></li>
//                     <li><NavLink to="/playroom/">Playroom</NavLink></li>
//                 </ul>
//             </nav>
//         );
//     }
// }
// export default Navbar;



import AppBar from '@material-ui/core/AppBar';
import Button from '@material-ui/core/Button';
import { makeStyles } from '@material-ui/core/styles';
import Toolbar from '@material-ui/core/Toolbar';
import Typography from '@material-ui/core/Typography';
import React from 'react';
import { Link } from 'react-router-dom';

const useStyles = makeStyles((theme) => ({
    root: {
        flexGrow: 1,
      },
    appBar: {
        backgroundColor: theme.palette.background.paper,
      //borderBottom: `1px solid ${theme.palette.divider}`,
    },
    toolbar: {
      flexWrap: 'wrap',
    },
    toolbarTitle: {
      flexGrow: 1,
    },
    link: {
      margin: theme.spacing(1, 1.5),
      textDecoration: 'none',
    },
    appbarmenu: {
        fontSize: "14px",
        //textTransform: "none",
        fontWeight: 400,
    },
    logo: {
        width: "30px",
        marginRight: "10px",
        verticalAlign: "middle"
    }
}));
export default function Navbar() {
    const classes = useStyles();
        return(
            <div className={classes.root}>
        <AppBar position="static" color="default" elevation={3} className={classes.appBar}>
        <Toolbar className={classes.toolbar}>
          <Typography variant="h6" color="inherit" noWrap className={classes.toolbarTitle}>
            League of Towers
          </Typography>
          <nav>

          <Link variant="button" color="textPrimary" exact to="/" className={classes.link}>
          <Button href="#text-buttons" color="primary">
                                    Home
                                </Button>
              
            </Link>

          <Link variant="button" color="textPrimary" exact to="/playroom/" className={classes.link}>
          <Button href="#text-buttons" color="primary">
                                    Playroom
                                </Button>
              
            </Link>
            <Link  to="/ranking/" className={classes.link}>
            <Button href="#text-buttons" color="primary">
              Ranking
              </Button>
            </Link>
            <Link to="/features/" className={classes.link}>
            <Button href="#text-buttons" color="primary">
              Features
              </Button>
            </Link>
            <Link to="/community/" className={classes.link}>
            <Button href="#text-buttons" color="primary">
              Community
              </Button>
            </Link>
            <Link to="/support/" className={classes.link}>
            <Button href="#text-buttons" color="primary">
              Support
              </Button>
            </Link>
          </nav>
          <Link to="/signin/" className={classes.link}>
          <Button href="#" color="primary" variant="outlined" className={classes.link}>
            Login
          </Button>
          </Link>
          <Link to="/signin/" className={classes.link}>
          <Button href="#" color="primary" variant="contained" className={classes.link}>
            Register
          </Button>
          </Link>
        </Toolbar>
      </AppBar>
      </div>
        );
}


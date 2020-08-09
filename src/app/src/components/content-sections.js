import React from 'react';
import Paper from '@material-ui/core/Paper';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import Paging from '../components/paging'
import About from '../components/about'
import Contact from '../components/contact'
import Experience from '../components/experience'
import Tech from '../components/tech'
import Slide from '@material-ui/core/Slide';

const useStyles = makeStyles((theme) => ({
    root: {
        // display: 'flex',
        // flexWrap: 'wrap',
        // '& > *': {
        //   margin: theme.spacing(1),
        //   width: theme.spacing(16),
        //   height: theme.spacing(16),
        // },
    },
}));

export default function ContentSections() {
    const classes = useStyles();
    const [activeContent, setActiveContent] = React.useState(0);
    const [pageDirection, setPageDirection] = React.useState('left');
    const [pageOutDirection, setPageOutDirection] = React.useState('right');
    const transition = { enter: 100, exit: 100, appear: 100};

    const handlePaging = (i) => {
        if(i > activeContent){
            setPageDirection('left');
            setPageOutDirection('right');
        }
        else{
            setPageDirection('right');
            setPageOutDirection('left');
        }
        setActiveContent((prev) => i);
      };

    const getPageDirection = (i) =>{
        return activeContent == i ? "left" : "right";
    }

    return (
        <div className={classes.root}>
            <Paging onClick={handlePaging} />
            <Slide direction={getPageDirection(0)} in={activeContent == 0} timeout={transition} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <About />
                </Paper>
            </Slide>
            <Slide direction={getPageDirection(1)} in={activeContent == 1} timeout={transition} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <Tech />
                </Paper>
            </Slide>
            <Slide direction={getPageDirection(2)} in={activeContent == 2} timeout={transition} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <Experience />
                </Paper>
            </Slide>
            <Slide direction={getPageDirection(3)} in={activeContent == 3} timeout={transition} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <Contact />
                </Paper>
            </Slide>
        </div>
    );
}
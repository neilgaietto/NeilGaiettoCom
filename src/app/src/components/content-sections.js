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

    const handlePaging = (i) => {
        if(i > activeContent){
            setPageDirection('left');
        }
        else{
            setPageDirection('right');
        }
        setActiveContent((prev) => i);
      };

    return (
        <div className={classes.root}>
            <Paging onClick={handlePaging} />
            <Slide direction={pageDirection} in={activeContent == 0} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <About />
                </Paper>
            </Slide>
            <Slide direction={pageDirection} in={activeContent == 1} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <Tech />
                </Paper>
            </Slide>
            <Slide direction={pageDirection} in={activeContent == 2} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <Experience />
                </Paper>
            </Slide>
            <Slide direction={pageDirection} in={activeContent == 3} mountOnEnter unmountOnExit>
                <Paper variant="outlined" elevation={1}>
                    <Contact />
                </Paper>
            </Slide>
        </div>
    );
}
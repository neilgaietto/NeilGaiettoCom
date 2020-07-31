import React from 'react';
import Paper from '@material-ui/core/Paper';
import { makeStyles, withStyles } from '@material-ui/core/styles';
import Paging from '../components/paging'
import About from '../components/about'
import Contact from '../components/contact'
import Experience from '../components/experience'
import Tech from '../components/tech'

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

  return (
    <div className={classes.root}>
      <Paging />
      <Paper variant="outlined" elevation={1}>
          <About />
      </Paper>
      <Paper variant="outlined" elevation={1}>
          <Tech />
      </Paper>
      <Paper variant="outlined" elevation={1}>
          <Experience />
      </Paper>
      <Paper variant="outlined" elevation={1}>
          <Contact />
      </Paper>

    </div>
  );
}
import React from 'react';
import { makeStyles, withStyles } from '@material-ui/core/styles';

const useStyles = makeStyles((theme) => ({
  root: {
  },
}));

export default function About() {
  const classes = useStyles();

  return (
    <div className={classes.root}>
        <h1>About</h1>
        <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam cursus quam porta nisi consectetur, ac tincidunt justo commodo. Nam iaculis mattis est, eget ornare elit lacinia non. Fusce tristique neque lacus. Aliquam rhoncus ullamcorper justo, eget eleifend lacus efficitur eget. Ut in finibus odio. In gravida urna quis lectus malesuada, vitae tempus purus eleifend. Curabitur vel orci et ipsum ultricies pulvinar sit amet non sem. Proin quis enim tempor, pharetra nisi eget, placerat neque. Aliquam feugiat tincidunt aliquam. Phasellus turpis sapien, mattis id augue a, commodo tincidunt nulla. Nullam bibendum elementum velit, sit amet lobortis est laoreet non.</p>
    </div>
  );
}
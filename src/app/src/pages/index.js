import React from 'react'

import Layout from '../components/layout'
import Image from '../components/image'
import SEO from '../components/seo'
import ContentSections from '../components/content-sections'

import { makeStyles } from '@material-ui/core/styles'
import Grid from '@material-ui/core/Grid'
import Divider from '@material-ui/core/Divider'
import Typography from '@material-ui/core/Typography'


const useStyles = makeStyles(theme => ({
  root: {
    width: '100%',
    backgroundColor: theme.palette.background.paper,
  },
  nested: {
    paddingLeft: theme.spacing(4),
  },
}));

const IndexPage = () => {

  const classes = useStyles()

  return (
    <Layout>
      <SEO title="Home" />
      <Grid container spacing={3} justify="center">
        <Grid item xs={2}>
          <div style={{ maxWidth: `500px` }}>
            <Image />
          </div>
        </Grid>
        <Grid item xs={8}>
          <h1>Neil Gaietto</h1>
          <h3>Software Architect and Developer</h3>
          <p>
            I am a software developer and architect that has been focused on C#, SQL, and Javascript solutions. The majority of my experience has been in developing large scalable sites and services that require high performance. I aspire to design the best solution for each of my clients unique challenges.
          </p>
        </Grid>
      </Grid>
      <Divider />
      <ContentSections />

    </Layout>
  )
}

export default IndexPage

import React from 'react';
import {
  AppBar,
  Toolbar,
  Button,
  Typography,
  Box,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';

const Navbar = () => {
  return (
    <AppBar position="static" sx={{ backgroundColor: 'white', color: 'black' }}>
      <Toolbar>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={() => {
            console.log('Create assignment clicked');
          }}
          sx={{ mr: 2 }}
        >
          Create Assignment
        </Button>

        <Box sx={{ flexGrow: 1 }} />

        <Typography variant="h6" component="div" sx={{ fontWeight: 600 }}>
          OmnIDE
        </Typography>
      </Toolbar>
    </AppBar>
  );
};

export default Navbar;
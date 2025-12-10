import React from "react";
import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile";
import { Box, Button, Divider, Typography } from "@mui/material";

export default function ProfileAbout() {
  const { id } = useParams();
  const { profile } = useProfile();

  return (
    <Box>
      <Box display="flex" justifyContent="space-between">
        <Typography variant="h5">About {profile?.displayName}</Typography>
        <Button onClick={() => console.log(id)}>Edit Profile</Button>
      </Box>
      <Divider sx={{ my: 2 }} />
      <Box sx={{ overflow: "auto", maxHeight: 350 }}>
        <Typography variant="body1" sx={{ whiteSpace: "pre-wrap" }}>
          {profile?.bio || "No description added yet"}
        </Typography>
      </Box>
    </Box>
  );
}

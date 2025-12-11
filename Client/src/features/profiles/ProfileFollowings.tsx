import { useParams } from "react-router";
import { useProfile } from "../../lib/hooks/useProfile";
import { Box, Divider, Typography } from "@mui/material";
import ProfileCard from "./ProfileCard";
import { useAccount } from "../../lib/hooks/useAccount";

type Props = {
  activeTab: number;
};

export default function ProfileFollowings({ activeTab }: Props) {
  const { id } = useParams();
  const predicate = activeTab === 3 ? "followers" : "followings";
  const { profile, followings, loadingFollowings } = useProfile(id, predicate);
  const { currentUser } = useAccount();

  return (
    <Box>
      <Box display="flex">
        <Typography variant="h5">
          {activeTab === 3
            ? `People following ${profile?.displayName}`
            : `People ${profile?.displayName} is following`}
        </Typography>
      </Box>
      <Divider sx={{ my: 2 }} />
      {loadingFollowings ? (
        <Typography>Loading...</Typography>
      ) : followings !== undefined && followings?.length > 0 ? (
        <Box display="flex" marginTop={3} gap={3}>
          {followings?.map((profile) => (
            <ProfileCard key={profile.id} profile={profile} />
          ))}
        </Box>
      ) : (
        <Typography>
          {activeTab === 3 && profile?.id !== currentUser?.id
            ? `${profile?.displayName} does not have any followers`
            : activeTab === 3 && profile?.id === currentUser?.id
            ? `You do not have any followers`
            : activeTab !== 3 && profile?.id !== currentUser?.id
            ? `${profile?.displayName} is not following anyone`
            : "You are not following anyone"}
        </Typography>
      )}
    </Box>
  );
}

import { Star, StarBorder } from "@mui/icons-material";
import { Box, Button } from "@mui/material";

type Props = {
  selected: boolean;
};

export default function StarButton({ selected }: Props) {
  return (
    <Box position="relative">
      <Button
        sx={{
          opacity: 0.8,
          transition: "opacity 0.3s",
          position: "relative",
          cursor: "pointer",
          paddingTop: 1,
          paddingLeft: 0,
          left: -4,
        }}
      >
        <StarBorder
          sx={{
            fontSize: 32,
            color: "white",
            position: "absolute",
          }}
        />
        <Star
          sx={{
            fontSize: 28,
            color: selected ? "yellow" : "rgba(0,0,0,0.5)",
          }}
        />
      </Button>
    </Box>
  );
}

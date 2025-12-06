import { z } from "zod";
import { requiredString } from "../util/util";

export const activitySchema = z.object({
  title: requiredString("Title"),
  description: requiredString("Description"),
  date: z.preprocess(
    (val) => {
      if (val instanceof Date) return val; // <-- FIX
      if (val === "") return undefined;
      return val;
    },
    z.coerce.date({
      error: "Date is required", // custom message
    })
  ),

  category: requiredString("Category"),
  location: z.object({
    venue: requiredString("Venue"),
    city: z.string().optional(),
    latitude: z.coerce.number(),
    longitude: z.coerce.number()
  })
});

export type ActivitySchema = z.input<typeof activitySchema>;

export interface FilterRequest {
  country: string | undefined;
  city: string  | undefined;
  minimumAvailableCarsNumber: number | undefined;
  startRent: Date | undefined;
  finishRent: Date | undefined;
}

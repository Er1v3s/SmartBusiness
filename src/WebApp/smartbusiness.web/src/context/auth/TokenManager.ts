/* eslint-disable @typescript-eslint/no-explicit-any */

export const removeAccessTokens = () => {
    localStorage.removeItem("ACCESS_TOKEN");
    localStorage.removeItem("ACCESS_TOKEN_EXPIRATION");
  };

export const setAccessTokens = (access_token : any, expiration_date : any) => {
    localStorage.setItem("ACCESS_TOKEN", access_token)
    localStorage.setItem("ACCESS_TOKEN_EXPIRATION", expiration_date)
};
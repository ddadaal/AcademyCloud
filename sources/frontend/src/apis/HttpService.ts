import axios from "axios";

export interface FetchInfo {
  path?: string;
  method?: HttpMethod;
  params?: { [key: string]: string | undefined };
  body?: { [key: string]: string | number | object };
  headers?: { [s: string]: string };
}

export enum HttpMethod {
  GET = "GET",
  POST = "POST",
  DELETE = "DELETE",
  PATCH = "PATCH",
  PUT = "PUT",
}

export interface HttpError {
  status: number;
  data?: unknown;
}

export function makeHttpError(status: number, data?: unknown): HttpError {
  return { status, data };
}


const BASE_URL = "http://localhost:5000";

const axiosInstance = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': "application/json " },
});

export class HttpService {

  protected get axios() {
    return axiosInstance;
  }

  /**
   * Execute fetch.
   * @param fetchInfo fetch paramters
   * @returns parsed response body if the request is successful
   * @throws A HttpError object if the request is not successful.
   */
  async fetch<T>(fetchInfo: FetchInfo): Promise<T> {
    try {
      const response = await axiosInstance({
        method: fetchInfo.method,
        url: fetchInfo.path,
        params: fetchInfo.params,
        data: fetchInfo.body,
      });
      return response.data;
    } catch (e) {
      if (e.response) {
        // request is complete but the status code is out of 200-300
        // throw it as HttpError
        throw e as HttpError;
      } else if (e.request) {
        // request is sent but no response
        // likely network error
        // throw -1
        throw {
          status: -1,
          data: null,
        };
      } else {
        // some config problem
        // throw -2
        throw {
          status: -2,
          data: null,
        };
      }
    }
  }
}


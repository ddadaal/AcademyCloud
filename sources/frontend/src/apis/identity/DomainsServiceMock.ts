import { DomainsService, GetDomainsResponse } from './DomainsService';

export class DomainsServiceMock extends DomainsService {
  async getDomains(): Promise<GetDomainsResponse> {
    return {
      domains: [
        {
          id: "NJUID",
          name: "NJUID",
          admins: [
            {
              id: "CJDID",
              name: "CJD",
            }, {
              id: "CJYID",
              name: "CJY"
            }
          ],
          resources: { cpu: 4, memory: 64, storage: 5000 },
        },
        {
          id: "PKUID",
          name: "PKU",
          admins: [{
            id: "CJDID",
            name: "CJD",
          }],
          resources: { cpu: 8, memory: 128, storage: 10000 },
        }
      ]
    };
  }
}

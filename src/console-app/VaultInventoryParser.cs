using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Glacier.Tools.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Glacier.Tools
{
    public class VaultInventoryParser
    {
        public static VaultInventory Parse(string vaultInventoryFile)
        {
            using (StreamReader streamReader = System.IO.File.OpenText(vaultInventoryFile))
            {
                var jsonReader = new JsonTextReader(streamReader);

                var root = (JObject) JToken.ReadFrom(jsonReader);

                var vaultArn = root["VaultARN"].Value<string>();

                DateTimeOffset inventoryDate = ParseDate2(root["InventoryDate"]);

                IOrderedEnumerable<GlacierFile> archiveList =
                    root["ArchiveList"].Select(a => new
                                                        {
                                                            Desc = JObject.Parse(a["ArchiveDescription"].Value<string>()),

                                                            DateArchived = ParseDate2(a["CreationDate"]),

                                                            ArchiveId = a["ArchiveId"].Value<string>()
                                                        })

                                       .Select(d => new GlacierFile(filePath: d.Desc["Path"].Value<string>(),

                                                                    dateModified: ParseDate1(d.Desc["UTCDateModified"]),

                                                                    sizeBytes: d.Desc["Size"].Value<long>(),

                                                                    dateArchived: d.DateArchived,
                                                                    
                                                                    archiveId: d.ArchiveId))

                                       .Where(g => g.IsBackupFile)

                                       .OrderBy(g => g.FilePath);

                jsonReader.Close();
                
                return new VaultInventory(vaultArn, inventoryDate, archiveList);
            }
        }

        static DateTimeOffset ParseDate1(IEnumerable<JToken> value)
        {
            DateTimeOffset date = DateTimeOffset.ParseExact(value.Value<string>(),
                                                            "yyyyMMdd'T'HHmmss'Z'",
                                                            CultureInfo.InvariantCulture,
                                                            DateTimeStyles.AssumeUniversal |
                                                            DateTimeStyles.AdjustToUniversal);

            return date;
        }

        static DateTimeOffset ParseDate2(IEnumerable<JToken> value)
        {
            DateTimeOffset date = DateTimeOffset.Parse(value.Value<string>(),
                                                       CultureInfo.InvariantCulture,
                                                       DateTimeStyles.AssumeUniversal |
                                                       DateTimeStyles.AdjustToUniversal);

            return date;
        }
    }
}

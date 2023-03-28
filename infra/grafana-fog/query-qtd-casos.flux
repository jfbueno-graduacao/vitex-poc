# Quantidade de possíveis casos de febre

from(bucket: "datalake_cloud")
  |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
  |> filter(fn: (r) => r["_measurement"] == "temperature")
  |> filter(fn: (r) => r["_value"] >= 38)
  |> group(columns: ["personId"])
  |> count()
  |> group()
  |> count()
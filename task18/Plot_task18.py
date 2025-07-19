import pandas as pd
import matplotlib.pyplot as plt
from datetime import datetime, timedelta

execution_log = [1, 2, 1, 2]

start = datetime(2025, 7, 19, 12, 0, 0)
timestamps = [start + timedelta(seconds=i) for i in range(len(execution_log))]

df = pd.DataFrame({
    "ts": timestamps,
    "id": execution_log
})

df.to_csv("task18_timeline.csv", index=False)

plt.figure(figsize=(10, 4))
for cmd_id in sorted(df["id"].unique()):
    pts = df[df["id"] == cmd_id]
    plt.scatter(pts["ts"], [cmd_id] * len(pts), s=60, label=f"Cmd {cmd_id}")

plt.title("Timeline исполнения Task18")
plt.xlabel("Время")
plt.ylabel("ID команды")
plt.legend(loc="upper right")
plt.tight_layout()
plt.savefig("task18_timeline.png", dpi=150)

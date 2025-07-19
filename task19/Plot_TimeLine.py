import pandas as pd
import matplotlib.pyplot as plt

df = pd.read_csv('timeline.csv', names=['ts','id'], parse_dates=['ts'])

plt.figure(figsize=(10, 4))
for cmd_id in sorted(df['id'].unique()):
    pts = df[df['id'] == cmd_id]
    plt.scatter(pts['ts'], [cmd_id]*len(pts), s=50, label=f'Cmd {cmd_id}')

plt.yticks(sorted(df['id'].unique()))
plt.xlabel('Время')
plt.ylabel('ID команды')
plt.title('Timeline выполнения TestCommand')
plt.legend(loc='upper right')
plt.tight_layout()
plt.savefig('timeline.png', dpi=150)
plt.show()
